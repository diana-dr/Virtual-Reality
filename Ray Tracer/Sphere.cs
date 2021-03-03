using System;

namespace rt
{
    public class Sphere : Geometry
    {
        private Vector Center { get; set; }
        private double Radius { get; set; }

        public Sphere(Vector center, double radius, Material material, Color color) : base(material, color)
        {
            Center = center;
            Radius = radius;
        }

        public override Intersection GetIntersection(Line line, double minDist, double maxDist)
        {
            // ADD CODE HERE: Calculate the intersection between the given line and this sphere

            var A = line.Dx * line.Dx;
            var B = line.Dx * (line.X0 - Center) * 2;
            var C = (line.X0 - Center) * (line.X0 - Center) - Radius * Radius;

            var delta = B * B - 4 * A * C;
            
            if (delta > 0)
            {
                double sqrtDelta = Math.Sqrt(delta);
                double t1 = (-B - sqrtDelta) / (2 * A);
                double t2 = (-B + sqrtDelta) / (2 * A);
                
                if (t1 >= minDist && t1 <= maxDist)
                    return new Intersection(true, true, this, line, t1);
                if (t2 >= minDist && t2 <= maxDist)
                    return new Intersection(true, true, this, line, t2);
            }
            else if (delta == 0)
            {
                double t = -B / (2 * A);

                if (t >= minDist && t <= maxDist)
                    return new Intersection(true, true, this, line, t);
                else
                    return new Intersection(true, false, this, line, t);
            }

            return new Intersection();
        }

        public override Vector Normal(Vector v)
        {
            var n = v - Center;
            n.Normalize();
            return n;
        }
    }
}