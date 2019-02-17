using innometrics_visual_studio.Model.Metrics;

namespace innometrics_visual_studio.Controller
{
    public abstract class AbstractActivityController:IActivity
    {
        private readonly string _activityType;

        private readonly string _executableName;

        public Metric Metric { get; }

        protected AbstractActivityController(string executableName, string activityType)
        {
            _executableName = executableName;
            _activityType = activityType;
            Metric = new Metric(_executableName, _activityType);
        }

        public abstract void OnChanged();

    }

    class LinesOfCodeActivityController : AbstractActivityController
    {

        public LinesOfCodeActivityController(string executableName) : base(executableName, "vs_lines_of_code"){}

        public override void OnChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}
