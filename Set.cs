using System;
using System.Collections.Generic;
using System.Linq;

namespace SetTheory0
{
    public class Set
    {
        public static readonly Set Empty = new Set();
        public static readonly Set N = new Set(x => x is int and > 0);
        public static readonly Set Z = new Set(x => x is int);
        public static readonly Set Q = new Set(x =>
        {
            switch (x)
            {
                case int:
                    return true;
                case double y:
                {
                    for (var i = 1; i < 30; i++)
                        if (Math.Abs(Math.Floor(y * i) - y * i) < 0.00000001)
                            return true;
                    break;
                }
            }

            return false;
        });
        public static readonly Set R = new Set(x => x is double or float or int);
        private static readonly HashSet<object> Samples;
        private static bool _ready;
        private readonly Func<object, bool> _has;
        private string _data;
        private static int _emptyCalls;
        private static int _samplesCalls;

        static Set()
        {
            Samples = GetSamples();
        }

        public Set(params object[] s)
        {
            /*
            ____________________________________
            |                                  |
            |           , - ~ ~ ~ - ,          |
            |       , '         s[4]  ' ,      |
            |     ,     s[0]     s[12]    ,    |
            |    ,          s[13]          ,   |
            |   ,     s[7]       s[2]       ,  |
            |   ,  s[3]    s[14]      s[9]  ,  |
            |   ,  s[11]           s[5]     ,  |
            |    ,       s[8]              ,   |
            |     , s[15]       s[1]      ,    |
            |       ,  s[6]      s[10] , '     |
            |         ' - , _ _ _ ,  '         |
            |                                  |
            |__________________________________|
            */
            if (s.Length == 0)
            {
                _has = _ => false;
                _data = "{}";
            }
            else
            {
                _has = x => ArrayContains(x, s);
                _data = "{";
                foreach (var element in s)
                {
                    _data += element + ", ";
                    if (_ready)
                        Samples.Add(element);
                }
                _data = _data.Substring(0, _data.Length - 2) + "}";
            }
        }

        public Set(Func<object, bool> has)
        {
            /*
            ____________________________________
            |                                  |
            |           , - ~ ~ ~ - ,          |
            |       , '               ' ,      |
            |     ,                       ,    |
            |    ,                         ,   |
            |   ,                           ,  |
            |   ,         has = true        ,  |
            |   ,                           ,  |
            |    ,                         ,   |
            |     ,                       ,    |
            |       ,                  , '     |
            |         ' - , _ _ _ ,  '         |
            |  has = false                     |
            |__________________________________|
            */
            _has = has;
            _data = "Set";
        }

        public Set From(Func<object, bool> has)
        {
            /*
            ________________________________________________________
            |                                                      |
            |           , - ~ ~ ~ - ,       , - ~ ~ ~ - ,          |
            |       , '               ' ,'                ' ,      |
            |     ,                   ,\\\,                   ,    |
            |    ,                   ,\\\\\,                   ,   |
            |   ,      this         ,\\\\\\\,                   ,  |
            |   ,                   ,\\new\\,                   ,  |
            |   ,                   ,\\\\\\\,                   ,  |
            |    ,                   ,\\\\\,      has = true   ,   |
            |     ,                   ,\\\,                   ,    |
            |       ,                  , '                   '     |
            |         ' - , _ _ _ ,  '     ' - , _ _ _ ,  '        |
            |                                                      |
            |______________________________________________________|
            */
            return new Set(x => this.Has(x) && has.Invoke(x));
        }

        public override string ToString()
        {
            if (_data != "Set")
                return _data;
            if (this.IsEmpty())
                return _data = "Ø";
            var sampled = MatchingSamples(this).Take(10);
            _data += " {";
            foreach (var sample in sampled)
                _data += sample + ", ";
            _data += "?...}";
            return _data;
        }

        public bool IsEmpty()
        {
            // Is the Set empty?
            // Due to infinity restrictions (ironic), this is not perfect. 'Best-effort'.
            _emptyCalls++;
            foreach (var sample in Samples)
                if (this.Has(sample))
                    return false;
            return true;
        }

        public bool Has(object x)
        {
            // x ∈ this
            
            /*
            ____________________________________
            |                                  |
            |     this: , - ~ ~ ~ - ,          |
            |       , '               ' ,      |
            |     ,                       ,    |
            |    ,                         ,   |
            |   ,                           ,  |
            |   ,                           ,  |
            |   ,                           ,  |
            |    ,     x                   ,   |
            |     ,                       ,    |
            |       ,                  , '     |
            |         ' - , _ _ _ ,  '         |
            |                                  |
            |__________________________________|
         */
            return _has.Invoke(x);
        }

        public bool IsSubsetOf(Set other)
        {
            // this ⊆ other
            /*
            ____________________________________
            |                                  |
            |    other: , - ~ ~ ~ - ,          |
            |       , '               ' ,      |
            |     ,                       ,    |
            |    ,                         ,   |
            |   ,      this:  x  x          ,  |
            |   ,          x        x       ,  |
            |   ,         x          x      ,  |
            |    ,        x          x     ,   |
            |     ,        x        x      ,   |
            |       ,         x  x     , '     |
            |         ' - , _ _ _ ,  '         |
            |                                  |
            |__________________________________|
            */
            return this.Difference(other).IsEmpty();
        }

        public bool IsSupersetOf(Set other)
        {
            // this ⊇ other
            
            /*
            ____________________________________
            |                                  |
            |     this: , - ~ ~ ~ - ,          |
            |       , '               ' ,      |
            |     ,                       ,    |
            |    ,                         ,   |
            |   ,      other: x  x          ,  |
            |   ,          x        x       ,  |
            |   ,         x          x      ,  |
            |    ,        x          x     ,   |
            |     ,        x        x      ,   |
            |       ,         x  x     , '     |
            |         ' - , _ _ _ ,  '         |
            |                                  |
            |__________________________________|
            */
            return other.IsSubsetOf(this);
        }

        public Set Union(Set other)
        {
            // this ∪ other
            // logical OR
            
            /*
            ________________________________________________________
            |                                                      |
            |           , - ~ ~ ~ - ,       , - ~ ~ ~ - ,          |
            |       , '\\\\\\\\\\\\\\\' ,'\\\\\\\\\\\\\\\\' ,      |
            |     ,\\\\\\\\\\\\\\\\\\\,\\\,\\\\\\\\\\\\\\\\\\\,    |
            |    ,\\\\\\\\\\\\\\\\\\\,\\\\\,\\\\\\\\\\\\\\\\\\\,   |
            |   ,\\\\\\\\\\\\\\\\\\\,\\\\\\\,\\\\\\\\\\\\\\\\\\\,  |
            |   ,\\\\\\\\\\\\\\\\\\\,\\\\\\\,\\\\\\\\\\\\\\\\\\\,  |
            |   ,\\\\\\\\\\\\\\\\\\\,\\\\\\\,\\\\\\\\\\\\\\\\\\\,  |
            |    ,\\\\\\\\\\\\\\\\\\\,\\\\\,\\\\\\\\\\\\\\\\\\\,   |
            |     ,\\\\\\\\\\\\\\\\\\\,\\\,\\\\\\\\\\\\\\\\\\\,    |
            |       ,\\\\\\\\\\\\\\\\\\, '\\\\\\\\\\\\\\\\\\\'     |
            |         ' - , _ _ _ ,  '     ' - , _ _ _ ,  '        |
            |                                                      |
            |______________________________________________________|
            */
            return new Set(x => this.Has(x) || other.Has(x));
        }
        public Set U(Set other) => this.Union(other);

        public Set Intersection(Set other)
        {
            // this ∩ other
            // logical AND
            
            /*
            ________________________________________________________
            |                                                      |
            |           , - ~ ~ ~ - ,       , - ~ ~ ~ - ,          |
            |       , '               ' ,'                ' ,      |
            |     ,                   ,\\\,                   ,    |
            |    ,                   ,\\\\\,                   ,   |
            |   ,                   ,\\\\\\\,                   ,  |
            |   ,                   ,\\\\\\\,                   ,  |
            |   ,                   ,\\\\\\\,                   ,  |
            |    ,                   ,\\\\\,                   ,   |
            |     ,                   ,\\\,                   ,    |
            |       ,                  , '                   '     |
            |         ' - , _ _ _ ,  '     ' - , _ _ _ ,  '        |
            |                                                      |
            |______________________________________________________|
            */
            return new Set(x => this.Has(x) && other.Has(x));
        }
        public Set n(Set other) => this.Intersection(other);

        public Set Difference(Set other)
        {
            // this - other
            // logic: (x ∈ this) and (x ∉ other)
            
            /*
            ________________________________________________________
            |                                                      |
            |           , - ~ ~ ~ - ,       , - ~ ~ ~ - ,          |
            |       , '\\\\\\\\\\\\\\\' ,'                ' ,      |
            |     ,\\\\\\\\\\\\\\\\\\\,   ,                   ,    |
            |    ,\\\\\\\\\\\\\\\\\\\,     ,                   ,   |
            |   ,\\\\\\\\\\\\\\\\\\\,       ,                   ,  |
            |   ,\\\\\\\\\\\\\\\\\\\,       ,                   ,  |
            |   ,\\\\\\\\\\\\\\\\\\\,       ,                   ,  |
            |    ,\\\\\\\\\\\\\\\\\\\,     ,                   ,   |
            |     ,\\\\\\\\\\\\\\\\\\\,   ,                   ,    |
            |       ,\\\\\\\\\\\\\\\\\\, '                   '     |
            |         ' - , _ _ _ ,  '     ' - , _ _ _ ,  '        |
            |                                                      |
            |______________________________________________________|
            */
            return new Set(x => this.Has(x) && !other.Has(x));
        }
        public static Set operator -(Set a, Set b)
        {
            return a.Difference(b);
        }
        
        public Set SymmetricDifference(Set other)
        {
            // this △ other
            // logical XOR
            // logic: (this U other) - (this ∩ other)
            
            /*
            ________________________________________________________
            |                                                      |
            |           , - ~ ~ ~ - ,       , - ~ ~ ~ - ,          |
            |       , '\\\\\\\\\\\\\\\' ,'\\\\\\\\\\\\\\\\' ,      |
            |     ,\\\\\\\\\\\\\\\\\\\,   ,\\\\\\\\\\\\\\\\\\\,    |
            |    ,\\\\\\\\\\\\\\\\\\\,     ,\\\\\\\\\\\\\\\\\\\,   |
            |   ,\\\\\\\\\\\\\\\\\\\,       ,\\\\\\\\\\\\\\\\\\\,  |
            |   ,\\\\\\\\\\\\\\\\\\\,       ,\\\\\\\\\\\\\\\\\\\,  |
            |   ,\\\\\\\\\\\\\\\\\\\,       ,\\\\\\\\\\\\\\\\\\\,  |
            |    ,\\\\\\\\\\\\\\\\\\\,     ,                   ,   |
            |     ,\\\\\\\\\\\\\\\\\\\,   ,                   ,    |
            |       ,\\\\\\\\\\\\\\\\\\, '                   '     |
            |         ' - , _ _ _ ,  '     ' - , _ _ _ ,  '        |
            |                                                      |
            |______________________________________________________|
            */
            return this.Union(other).Difference(this.Intersection(other));
        }

        // Venn diagram here?
        public Set Power()
        {
            // P(this)
            // = {x | x ⊆ this}
            return new Set(x => x is Set set && set.IsSubsetOf(this));
        }

        public Set Complement()
        {
            // this`
            // logical NOT
            
            /*
            ____________________________________
            |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|
            |\this`\\\\\, - ~ ~ ~ - ,\\\\\\\\\\|
            |\\\\\\\, '               ' ,\\\\\\|
            |\\\\\,                       ,\\\\|
            |\\\\,                         ,\\\|
            |\\\,                           ,\\|
            |\\\,          this             ,\\|
            |\\\,                           ,\\|
            |\\\\,                         ,\\\|
            |\\\\\,                       ,\\\\|
            |\\\\\\\,                  , '\\\\\|
            |\\\\\\\\\' - , _ _ _ ,  '\\\\\\\\\|
            |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|
            |__________________________________|
         */
            return new Set(x => !this.Has(x));
        }
        public static Set operator !(Set a) => a.Complement();

        public override bool Equals(object o)
        {
            // this = other
            // = this ⊆ other and this ⊇ other
            
            /*
            ____________________________________
            |      other:                      |
            |    this:  , - ~ ~ ~ - ,          |
            |       , '\\\\\\\\\\\\\\\' ,      |
            |     ,\\\\\\\\\\\\\\\\\\\\\\\,    |
            |    ,\\\\\\\\\\\\\\\\\\\\\\\\\,   |
            |   ,\\\\\\\\\\\\\\\\\\\\\\\\\\\,  |
            |   ,\\\\\\\\\\\\\\\\\\\\\\\\\\\,  |
            |   ,\\\\\\\\\\\\\\\\\\\\\\\\\\\,  |
            |    ,\\\\\\\\\\\\\\\\\\\\\\\\\,   |
            |     ,\\\\\\\\\\\\\\\\\\\\\\\,    |
            |       ,\\\\\\\\\\\\\\\\\\, '     |
            |         ' - , _ _ _ ,  '         |
            |                                  |
            |__________________________________|
            */
            if (o is Set other)
                return this.IsSubsetOf(other) && other.IsSubsetOf(this);
            return false;
        }
        public static bool operator ==(Set s1, Set s2)
        {
            if (ReferenceEquals(s1, s2)) 
                return true;
            if (ReferenceEquals(s1, null)) 
                return false;
            if (ReferenceEquals(s2, null))
                return false;
            return s1.Equals(s2);
        }
        public static bool operator !=(Set s1, Set s2) => !(s1 == s2);
        
        public bool IsStrictSubsetOf(Set other)
        {
            // this ⊂ other
            return this.IsSubsetOf(other) && !this.Equals(other);
        }
        
        public bool IsStrictSupersetOf(Set other)
        {
            // this ⊃ other
            return this.IsSupersetOf(other) && !this.Equals(other);
        }

        public int Cardinality()
        {
            // |this| if finite
            // if presumably infinite (> 50 elements), returns -1.
            var size = MatchingSamples(this).Take(51).Count();
            return size > 50 ? -1 : size;
        }
        

        public static Set CartesianProduct(Set a, Set b)
        {
            // A×B
            // = {(a, b) | a ∈ A and b ∈ B}
            
            // (a, b) := {{a, true}, {b, false}}
            AddPairSamples(a, b);
            return new Set(p =>
                {
                    if (p is not Pair pair) return false;
                    return a.Has(pair.A) && b.Has(pair.B);
                }
            );
        }

        public static Set Union(params Set[] sets)
        {
            var result = new Set();
            foreach (var set in sets)
            {
                result = result.Union(set);
            }

            return result;
        }
        public static Set operator +(Set a, Set b)
        {
            return Union(a, b);
        }
        public static Set operator |(Set a, Set b)
        {
            return Union(a, b);
        }
        
        public static Set Intersection(params Set[] sets)
        {
            var result = new Set(_ => true);
            foreach (var set in sets)
            {
                result = result.Intersection(set);
            }

            return result;
        }
        public static Set operator *(Set a, Set b)
        {
            return Intersection(a, b);
        }
        public static Set operator &(Set a, Set b)
        {
            return Intersection(a, b);
        }

        public static Set SymmetricDifference(params Set[] sets)
        {
            var result = new Set();
            foreach (var set in sets)
            {
                result = result.Intersection(set);
            }

            return result;
        }
        public static Set operator ^(Set a, Set b)
        {
            return SymmetricDifference(a, b);
        }

        private static bool ArrayContains(object x, object[] array)
        {
            return array.Any(a => a == x || a.Equals(x));
        }

        private static HashSet<object> GetSamples()
        {
            var result = new HashSet<object>[4];
            // Initialise all HashSets
            for (int i = 0; i < result.Length; i++)
                result[i] = new();
            // Transform into more readable, customisable code:
            // result[0]
            result[0].Add(Set.Empty);
            result[0].Add(true);
            result[0].Add(false);
            for (int num = -99; num < 100; num++)
                result[0].Add(num);
            for (int num = -99; num < 99; num++)
                result[0].Add(num + 0.5);
            // Add more layers (result[1..N])
            for (int i = 1; i < result.Length; i++)
                foreach (var element in result[i-1])
                    result[i].Add(new Set(element));
            // Add one iteration of `Power`, if the length is short enough.
            // Merge All:
            var all = new HashSet<object>();
            foreach (var layer in result)
                all.UnionWith(layer);
            _ready = true;
            return all;
        }

        public static void PrintSampleSpace()
        {
            if (Samples.Count < 1000)
            {
                Console.WriteLine("Sample Space (U):\n");
                foreach (var element in Samples)
                    if (element is int[] arr)
                        Console.WriteLine("    [" + String.Join(", ", arr) + "]");
                    else
                        Console.WriteLine("    " + element);
            }
            else
            {
                Console.Write("Sample Space (U): {...} ");
            }
            Console.WriteLine("Length=" + Samples.Count);
            Console.WriteLine();
        }

        private static void AddPairSamples(Set a, Set b)
        {
            var ays = MatchingSamples(a);
            var bis = MatchingSamples(b);
            foreach (var x in ays)
                foreach (var y in bis)
                    Samples.Add(new Pair(x, y));
        }

        private static List<object> MatchingSamples(Set x)
        {
            _samplesCalls++;
            return Samples.Where(x.Has).Distinct().ToList();
        }

        public static void PrintEmptyCalls()
        {
            Console.WriteLine($"Set().IsEmpty() was called {_emptyCalls} time(s).");
            Console.WriteLine($"Set().MatchingSamples() was called {_samplesCalls} time(s).");
            // Console.WriteLine($"Sample Space is now {Samples.Count} items long.");
        }

        public static void PrintInterface()
        {
            Console.WriteLine("*** SetTheory0.Set Interface: ***");
            Console.WriteLine("  Constructors:");
            Console.WriteLine("    new Set(object[])");
            Console.WriteLine("    new Set(element, element, element...)");
            Console.WriteLine("    new Set()");
            Console.WriteLine("    new Set(object x => condition)");
            Console.WriteLine("  Constants:");
            Console.WriteLine("    Set.Empty = The Empty Set");
            Console.WriteLine("    Set.N = Natural numbers");
            Console.WriteLine("    Set.Z = Integer numbers");
            Console.WriteLine("    Set.R = Real numbers");
            Console.WriteLine("  Methods:");
            Console.WriteLine("    string ToString()");
            Console.WriteLine("    bool IsEmpty()");
            Console.WriteLine("    bool Has(object)");
            Console.WriteLine("    bool IsSubsetOf(Set)");
            Console.WriteLine("    bool IsSupersetOf(Set)");
            Console.WriteLine("    bool Equals(Set)");
            Console.WriteLine("    bool IsStrictSubsetOf(Set)");
            Console.WriteLine("    bool IsStrictSupersetOf(Set)");
            Console.WriteLine("    int Cardinality()");
            Console.WriteLine("    Set From(additionalCondition)");
            Console.WriteLine("    Set Union(Set)");
            Console.WriteLine("        or Set U(Set)");
            Console.WriteLine("    Set Intersection(Set)");
            Console.WriteLine("        or Set n(Set)");
            Console.WriteLine("    Set Difference(Set)");
            Console.WriteLine("        or a - b");
            Console.WriteLine("    Set SymmetricDifference(Set)");
            Console.WriteLine("    Set Power()");
            Console.WriteLine("    Set Complement()");
            Console.WriteLine("        or !a");
            Console.WriteLine("  Static Methods:");
            Console.WriteLine("    static void PrintSampleSpace()");
            Console.WriteLine("    static void PrintEmptyCalls()");
            Console.WriteLine("    static bool ==");
            Console.WriteLine("    static bool !=");
            Console.WriteLine("    static Set Union(Set, Set, Set...)");
            Console.WriteLine("        or a + b");
            Console.WriteLine("        or a | b");
            Console.WriteLine("    static Set Intersection(Set, Set, Set...)");
            Console.WriteLine("        or a * b");
            Console.WriteLine("        or a & b");
            Console.WriteLine("    static Set SymmetricDifference(Set, Set, Set...)");
            Console.WriteLine("        or a ^ b");
            Console.WriteLine("    static Set CartesianProduct(Set, Set)");
            
        }

        // Venn Diagrams (Empty)
        /*
            ________________________________________________________
            |                                                      |
            |           , - ~ ~ ~ - ,       , - ~ ~ ~ - ,          |
            |       , '               ' ,'                ' ,      |
            |     ,                   ,   ,                   ,    |
            |    ,                   ,     ,                   ,   |
            |   ,                   ,       ,                   ,  |
            |   ,                   ,       ,                   ,  |
            |   ,                   ,       ,                   ,  |
            |    ,                   ,     ,                   ,   |
            |     ,                   ,   ,                   ,    |
            |       ,                  , '                   '     |
            |         ' - , _ _ _ ,  '     ' - , _ _ _ ,  '        |
            |                                                      |
            |______________________________________________________|
         */
        
        /*
            ____________________________________
            |                                  |
            |           , - ~ ~ ~ - ,          |
            |       , '               ' ,      |
            |     ,                       ,    |
            |    ,                         ,   |
            |   ,                           ,  |
            |   ,                           ,  |
            |   ,                           ,  |
            |    ,                         ,   |
            |     ,                       ,    |
            |       ,                  , '     |
            |         ' - , _ _ _ ,  '         |
            |                                  |
            |__________________________________|
         */
        
        /*
            ____________________________________
            |                                  |
            |           , - ~ ~ ~ - ,          |
            |       , '               ' ,      |
            |     ,                       ,    |
            |    ,                         ,   |
            |   ,             x  x          ,  |
            |   ,          x        x       ,  |
            |   ,         x          x      ,  |
            |    ,        x          x     ,   |
            |     ,        x        x      ,   |
            |       ,         x  x     , '     |
            |         ' - , _ _ _ ,  '         |
            |                                  |
            |__________________________________|
            */
    }
}