namespace AddonElement
{
    public class WidgetEditLine : WidgetEditBase
    {
        public bool isPassword { get; set; }
        public string ReactionEnter { get; set; }
        public string reactionUp { get; set; }
        public string reactionDown { get; set; }

        public WidgetEditLine()
        {
            isPassword = false;
        }
    }
}
