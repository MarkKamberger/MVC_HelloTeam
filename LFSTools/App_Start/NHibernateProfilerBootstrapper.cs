using HibernatingRhinos.Profiler.Appender.NHibernate;

[assembly: WebActivator.PreApplicationStartMethod(typeof(LFSTools.App_Start.NHibernateProfilerBootstrapper), "PreStart")]
namespace LFSTools.App_Start
{
	public static class NHibernateProfilerBootstrapper
	{
        /// <summary>
        /// Pres the start.
        /// </summary>
		public static void PreStart()
		{
			NHibernateProfiler.Initialize();
		}
	}
}

