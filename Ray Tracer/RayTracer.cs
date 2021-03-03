using System;
using System.Runtime.InteropServices;

namespace rt
{
    class RayTracer
    {
        private Geometry[] geometries;
        private Light[] lights;

        public RayTracer(Geometry[] geometries, Light[] lights)
        {
            this.geometries = geometries;
            this.lights = lights;
        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        private Intersection FindFirstIntersection(Line ray, double minDist, double maxDist)
        {
            var intersection = new Intersection();

            foreach (var geometry in geometries)
            {
                var intr = geometry.GetIntersection(ray, minDist, maxDist);

                if (!intr.Valid || !intr.Visible) continue;

                if (!intersection.Valid || !intersection.Visible)
                {
                    intersection = intr;
                }
                else if (intr.T < intersection.T)
                {
                    intersection = intr;
                }
            }

            return intersection;
        }

        private bool IsLit(Vector point, Light light)
        {
            var dist = (light.Position - point).Length();
            var ray = new Line(point, light.Position);

            foreach (var geometry in geometries)
            {
                var intr = geometry.GetIntersection(ray, 0, dist);
                if (intr.Visible && intr.T > 0.001)
                    return false;
            }
            
            return true;
        }

        public void Render(Camera camera, int width, int height, string filename)
        {
            var background = new Color();
            var image = new Image(width, height);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    // ADD CODE HERE: Implement pixel color calculation
            
                    var parallel = camera.Up ^ camera.Direction;
                    var x1 = camera.Position + camera.Direction * camera.ViewPlaneDistance +
                             camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneWidth) +
                             parallel * ImageToViewPlane(i, width, camera.ViewPlaneHeight);

                    Intersection intersection = FindFirstIntersection(new Line(camera.Position, x1),
                        camera.FrontPlaneDistance,
                        camera.BackPlaneDistance);
                    
                    var color = new Color();
                    
                    if (intersection.Valid && intersection.Visible)
                    {
                        background = intersection.Geometry.Material.Ambient;
                        var N = intersection.Geometry.Normal(intersection.Position);
                        var E = (camera.Position - intersection.Position).Normalize();
                        
                        foreach (var light in lights)
                        {
                            color = intersection.Geometry.Material.Ambient * light.Ambient;
                    
                            if (IsLit(intersection.Position, light))
                            {
                                var T = (light.Position - intersection.Position).Normalize();
                                var R = (N * (N * T) * 2 - T).Normalize();
                                
                                if (N * T > 0)
                                    color += intersection.Geometry.Material.Diffuse * light.Diffuse * (N * T);
                                
                                if (E * R > 0)
                                    color += intersection.Geometry.Material.Specular * light.Specular *
                                             Math.Pow(E * R, intersection.Geometry.Material.Shininess);
                                
                            }
                            color *= light.Intensity;
                            background += color;
                        }
                        image.SetPixel(i, j, background);
                    }
                    else
                    {
                        image.SetPixel(i, j, color);
                    }
                }
            }
            image.Store(filename);
        }
    }
}