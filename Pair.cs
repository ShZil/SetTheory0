namespace SetTheory0
{
    class Pair
    {
        private object a;
        private object b;

        public Pair(object a, object b)
        {
            this.a = a;
            this.b = b;
        }

        public object A => a;

        public object B => b;

        public override string ToString()
        {
            return $"({a}, {b})";
        }
    }
}