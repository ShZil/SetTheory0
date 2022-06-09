using System;

namespace SetTheory0
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Set.PrintInterface();
            Set.PrintSampleSpace();

            /*
            // Basic check, toying with Naturals, Even/Odd/
            Set even = Set.N.From(x => (int)x % 2 == 1);
            Set odd1 = Set.N.Difference(even);
            Set odd2 = Set.N.From(x => (int)x % 2 == 1);
            Console.WriteLine("(Natural - Even) = Odd? " + odd1.Equals(odd2));
            Console.WriteLine("2 in Even? " + even.Has(2));
            Console.WriteLine("3 in Natural - Even? " + odd1.Has(3));
            */

            /*
            // Ron's
            Set s1 = Set.Empty;
            Set s2 = new Set(s1);
            Set s3 = new Set(s2);
            Set s4 = new Set(s3);

            Set a = new Set(s1, s2, s3);
            Set b = new Set(s2, s3, s4);

            Set aub = a.Union(b);
            Set anb = a.Intersection(b);

            Console.WriteLine(">> Should all be true: <<");
            Console.WriteLine(aub.Has(s1));
            Console.WriteLine(aub.Has(s2));
            Console.WriteLine(aub.Has(s3));
            Console.WriteLine(aub.Has(s4));

            Console.WriteLine(">> Should be false, true, true, false: <<");
            Console.WriteLine(anb.Has(s1));
            Console.WriteLine(anb.Has(s2));
            Console.WriteLine(anb.Has(s3));
            Console.WriteLine(anb.Has(s4));
            */

            /*
            // Set theory basics (from website):
            Set A = Set.N.From(x => (int) x < 68);
            Set B = Set.N.From(x => (int) x < 77);
            Set C = Set.Z.From(x => -30 < (int) x && (int) x < 40);
            
            Console.WriteLine(">> Should all be true: <<");
            Console.WriteLine(A.U(B.U(C)) == (A.U(B)).U(C));
            Console.WriteLine(A.n(B.n(C)) == (A.n(B)).n(C));
            
            Console.WriteLine(A.U(B) == B.U(A));
            Console.WriteLine(A.n(B) == B.n(A));
            
            Console.WriteLine(A.U(B.n(C)) == (A.U(B)).n(A.U(C)));
            Console.WriteLine(A.n(B.U(C)) == (A.n(B)).U(A.n(C)));

            Console.WriteLine(A.U(A) == A);
            Console.WriteLine(A.n(A) == A);
            Console.WriteLine(A.U(Set.Empty) == A);
            Console.WriteLine(A.n(Set.Empty) == Set.Empty);
            Console.WriteLine(A.n(Set.Empty) == Set.Empty);
            Console.WriteLine(A.Difference(A) == Set.Empty);

            if (A.IsSubsetOf(B))
            {
                Console.WriteLine(A.U(B) == A.U(B.Difference(A)));
                Console.WriteLine(A.U(B) == B);
                Console.WriteLine(A.n(B) == A);
            }
            */

            /*
            // Relations between sets of numbers
            Console.WriteLine("Set; Set; false; false; true; false; true:");
            Console.WriteLine(Set.R);
            Console.WriteLine(Set.Z);
            Console.WriteLine(Set.R == Set.Z);
            Console.WriteLine(Set.R.IsSubsetOf(Set.Z));
            Console.WriteLine(Set.Z.IsSubsetOf(Set.R));
            Console.WriteLine(Set.R.IsStrictSubsetOf(Set.Z));
            Console.WriteLine(Set.Z.IsStrictSubsetOf(Set.R));
            Console.WriteLine();
            Console.WriteLine(Set.Q.Has(1.0 / 2));
            Console.WriteLine(Set.Q.Has(1.0 / 3));
            Console.WriteLine(Set.Q.Has(2.0 / 3));
            Console.WriteLine(Set.Q.Has(1.0 / 4));
            Console.WriteLine(Set.Q.Has(2.0 / 4));
            Console.WriteLine(Set.Q.Has(3.0 / 4));
            Console.WriteLine(Set.Q.Has(4.0 / 4));
            Console.WriteLine(Set.Q.Has(1.0 / 1));
            Console.WriteLine(Set.Q.Has(0.00001));
            */

            // Cartesian Product:
            Console.WriteLine("Expected: (1, red); Set; {red, black, green, blue}; Set; true:");
            Pair pair = new Pair(1, "red");
            Set numbers = new Set(x => x is > 0 and < 9); // {1, 2, 3, 4, 5, 6, 7, 8}
            Set colors = new Set("red", "black", "green", "blue");
            Console.WriteLine("pair: " + pair);
            Console.WriteLine("set numbers: " + numbers);
            Console.WriteLine("|numbers| = " + numbers.Cardinality());
            Console.WriteLine("set colors: " + colors);
            Console.WriteLine("|colors| = " + colors.Cardinality());
            Set product = Set.CartesianProduct(numbers, colors);
            Console.WriteLine("set product: " + product);
            Console.WriteLine("pair in product? " + product.Has(pair));

            Console.WriteLine();
            Set.PrintEmptyCalls();
            Set.PrintSampleSpace();
        }
    }
}
