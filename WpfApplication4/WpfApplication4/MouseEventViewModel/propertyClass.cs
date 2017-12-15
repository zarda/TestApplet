namespace MouseOverEventMVVM
{
    public class WFP_Property
    {
        public int fontSize { get; set; }
        public string name{ get; set; }
        public WFP_Property(string _name, int _fontSize)
        {
            name = _name;
            fontSize = _fontSize;
        }
    }
}
