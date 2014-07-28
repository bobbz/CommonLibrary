using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace CommonLibrary
{
    public class PowerShellManager : IDisposable
    {

        protected Runspace m_runspace;

        public PowerShellManager()
        {
            InitializeRunSpace();
        }

        protected virtual void InitializeRunSpace()
        {
            m_runspace = RunspaceFactory.CreateRunspace();
            m_runspace.Open();
        }

        public bool Execute(out List<string> results, params string[] commands)
        {
            using (Pipeline pipe = m_runspace.CreatePipeline())
            {
                results = new List<string>();
                // Create the script
                foreach (string line in commands)
                {
                    //TraceFormat.WriteLine(line);
                    pipe.Commands.AddScript(line);
                }

                // Run the script
                try
                {
                    Collection<PSObject> psresults = pipe.Invoke();

                    // Get the results of the script's execution
                    foreach (PSObject result in psresults)
                    {
                        results.Add(result.ToString());
                        //TraceFormat.WriteLine("PowerShell result = {0}", result.ToString());
                    }

                }
                catch (Exception e)
                {
                    //TraceFormat.WriteLine(e.Message);
                    //TraceFormat.WriteLine(e.StackTrace);
                    return false;
                }
            }
            return true;//ExitCodes.Success;
        }

        #region Dispose pattern

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_runspace != null)
                {
                    m_runspace.Dispose();
                    m_runspace = null;
                }
            }

        }

        #endregion

    }
}
