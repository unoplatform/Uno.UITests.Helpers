using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace UITests.Queries
{
    public static class Helpers
    {
        public static class Android
        {
            public static void PressDefaultUserAction() => Helpers.OnAndroid(delegate (AndroidApp app)
                                                           {
                                                               app.PressUserAction(null);
                                                           });

            public static void PressUserAction(UserAction action) => Helpers.OnAndroid(delegate (AndroidApp app)
                                                                     {
                                                                         app.PressUserAction(new UserAction?(action));
                                                                     });
        }

        private static IApp app;

        private static int screenshotSuspensions = 0;

        private static bool? runningOnSimulator;

        public static IApp App
        {
            get
            {
                Assert.IsNotNull(Helpers.app, "Helpers.App must be set before test code runs. See http://bit.ly/1X7qeOA.");
                return Helpers.app;
            }
            set
            {
                Helpers.app = value;
            }
        }

        public static bool ScreenshotsSuspended => 0 < Helpers.screenshotSuspensions;

        public static QueryEx Any => QueryEx.Any;

        public static QueryEx Visible => QueryEx.Visible;

        public static QueryEx Button => QueryEx.Button;

        public static QueryEx Table => QueryEx.Table;

        public static QueryEx Cell => QueryEx.Cell;

        public static QueryEx Entry => QueryEx.Entry;

        public static QueryEx Switch => QueryEx.Switch;

        public static QueryEx Label => QueryEx.Label;

        public static BackdoorProxy Backdoor => new BackdoorProxy("Xamarin.UITest.Backdoor");

        public static bool RunningOnSimulator
        {
            get
            {
                bool? flag = Helpers.runningOnSimulator;
                if (!flag.HasValue)
                {
                    string input = Helpers.App.TestServer.Get("version");
                    Helpers.runningOnSimulator = new bool?(Regex.IsMatch(input, "\"simulator_device\":\"(.+)\""));
                }
                return Helpers.runningOnSimulator.Value;
            }
        }

        public static Platform Platform
        {
            get
            {
                if (Helpers.App is iOSApp)
                {
                    return Platform.iOS;
                }
                if (Helpers.App is AndroidApp)
                {
                    return Platform.Android;
                }
                throw new Exception("Current platform cannot be determined");
            }
        }

        private static void DoNothing()
        {
        }

        private static Action Void<T>(Func<T> f) => delegate
                                                              {
                                                                  f();
                                                              };

        public static void SuspendScreenshots() => Helpers.screenshotSuspensions++;

        public static void ResumeScreenshots() => Helpers.screenshotSuspensions--;

        public static void WithScreenshotsSuspended(Action act)
        {
            Helpers.SuspendScreenshots();
            act();
            Helpers.ResumeScreenshots();
        }

        public static T WithScreenshotsSuspended<T>(Func<T> f)
        {
            T t = default(T);
            Helpers.WithScreenshotsSuspended<T>(() => t = f());
            return t;
        }

        public static void Screenshot(string label)
        {
            if (Helpers.ScreenshotsSuspended)
            {
                return;
            }
            Helpers.App.Screenshot(label);
        }

        public static void Interact() => Helpers.App.Repl();

        public static void PressEnter() => Helpers.App.PressEnter();

        public static void DismissKeyboard() => Helpers.App.DismissKeyboard();

        public static QueryEx Raw(string s) => Helpers.Any.Raw(s);

        public static void Back() => Helpers.App.Back();

        public static void Wait(TimeSpan time) => Thread.Sleep(time);

        public static void Wait(int seconds) => Helpers.Wait(TimeSpan.FromSeconds((double)seconds));

        public static void Wait(float seconds) => Helpers.Wait(TimeSpan.FromSeconds((double)seconds));

        public static void WaitUntilExists(params QueryEx[] queries)
        {
            for (int i = 0; i < queries.Length; i++)
            {
                QueryEx query = queries[i];
                Helpers.App.WaitForElement(query, "Timed out waiting for element...", null, null, null);
            }
        }

        public static void First(string title, Action actions = null) => Helpers.Step(title, actions, "First");

        public static void First<T>(string title, Func<T> f) => Helpers.First(title, Helpers.Void<T>(f));

        public static void Then(string title, Action actions = null) => Helpers.Step(title, actions, "Then");

        public static void Then<T>(string title, Func<T> f) => Helpers.Then(title, Helpers.Void<T>(f));

        public static void Step(string title, Action actions = null, string prefix = "")
        {
            actions = actions ?? Helpers.DoNothing;
            actions();
            Helpers.Screenshot(string.Format("{0} {1}", prefix, title));
        }

        public static void ThenTap(string marked) => Helpers.ThenTap(marked, Helpers.Any.Marked(marked));

        public static void ThenTap(string label, QueryEx query) => Helpers.Then<QueryEx>(string.Format("I tap '{0}'", label), () => query.Tap());

        public static void Tap(QueryEx query) => query.Tap();

        public static QueryEx ScrollDownTo(QueryEx query) => query.ScrollDownTo();

        public static QueryEx ScrollUpTo(QueryEx query) => query.ScrollUpTo();

        public static T On<T>(T iOS, T Android)
        {
            Platform platform = Helpers.Platform;
            if (platform == Platform.Android)
            {
                return Android;
            }

            if (platform != Platform.iOS)
            {
                throw new ArgumentOutOfRangeException();
            }

            return iOS;
        }

        public static void OniOS(Action act) => Helpers.On<Action>(act, Helpers.DoNothing)();

        public static void OnAndroid(Action act) => Helpers.On<Action>(Helpers.DoNothing, act)();

        public static void OniOS<T>(Func<T> f) => Helpers.OniOS<T>(() => f());

        public static void OnAndroid<T>(Func<T> f) => Helpers.OnAndroid<T>(() => f());

        public static void OniOS(Action<iOSApp> act) => Helpers.OniOS(() =>
                                                        {
                                                            act(Helpers.App as iOSApp);
                                                        });

        public static void OnAndroid(Action<AndroidApp> act) => Helpers.OnAndroid(() =>
                                                                {
                                                                    act(Helpers.App as AndroidApp);
                                                                });

        public static void OniOS<T>(Func<iOSApp, T> f) => Helpers.OniOS<T>((iOSApp app) => f(app));

        public static void OnAndroid<T>(Func<AndroidApp, T> f) => Helpers.OnAndroid<T>((AndroidApp app) => f(app));

        public static void ShouldBeVisible(params QueryEx[] queries)
        {
            for (int i = 0; i < queries.Length; i++)
            {
                QueryEx query = queries[i];
                query.ShouldBeVisible(null);
            }
        }

        public static void ShouldNotBeVisible(params QueryEx[] queries)
        {
            for (int i = 0; i < queries.Length; i++)
            {
                QueryEx query = queries[i];
                query.ShouldNotBeVisible(null);
            }
        }
    }
}
