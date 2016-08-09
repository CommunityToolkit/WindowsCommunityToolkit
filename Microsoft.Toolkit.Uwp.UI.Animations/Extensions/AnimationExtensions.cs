using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods perform animation on UIElements
    /// </summary>
    public static partial class AnimationExtensions
    {
        private static EasingFunctionBase _defaultStoryboardEasingFunction = new CubicEase();

        /// <summary>
        /// Begins a Storyboard animation and returns a task that completes when the
        /// animaton is complete
        /// </summary>
        /// <param name="storyboard">The storyoard to be started</param>
        /// <returns>Task that completes when the animation is complete</returns>
        public static Task BeginAsync(this Storyboard storyboard)
        {
            var taskSource = new TaskCompletionSource<object>();
            EventHandler<object> completed = null;
            completed += (s, e) =>
            {
                storyboard.Completed -= completed;
                taskSource.SetResult(null);
            };

            storyboard.Completed += completed;
            storyboard.Begin();

            return taskSource.Task;
        }
    }
}
