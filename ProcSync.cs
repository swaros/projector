using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    public class ProcSync
    {
        private static Hashtable procs = new Hashtable();

        public static int ProcCount = 0;

        public static List<string> getAllMainProcs(){
            List<string> localProcs = new List<string>();
            foreach (DictionaryEntry proc in ProcSync.procs)
            {
                localProcs.Add(proc.Key.ToString());
            }
            return localProcs;
        }

        public static void Reset()
        {
            ProcSync.procs.Clear();
            ProcCount = 0;
        }

        public static void Reset(string name)
        {
            if (ProcSync.procs.ContainsKey(name))
            {
                int cnt = ProcSync.getProcCount(name);
                ProcSync.procs.Remove(name);
                ProcCount -= cnt;
            }
        }

        public static Boolean registerProc(String name){
            if (ProcSync.procs.ContainsKey(name))
            {
                return false;
            }
            ProcSync.procs.Add(name, new List<string>());
            return true;
        }

        public static Boolean isRegistered(string name){
            return ProcSync.procs.ContainsKey(name);
        }

        public static Boolean addSubProc(string main, string procname)
        {
            if (!ProcSync.procs.ContainsKey(main))
            {
                return false;
            }

            List<string> runnings = (List<string>) ProcSync.procs[main];
            runnings.Add(procname);
            ProcSync.procs[main] = runnings;
            ProcSync.ProcCount++;

            return true;
        }

        public static Boolean removeMainProc(string main)
        {
            if (!ProcSync.procs.ContainsKey(main))
            {
                return false;
            }

            ProcSync.procs.Remove(main);
            return true;
        }

        public static Boolean removeSubProc(string main, string procname)
        {
            if (!ProcSync.procs.ContainsKey(main))
            {
                return false;
            }

            List<string> runnings = (List<string>)ProcSync.procs[main];
            for (int i = 0; i < runnings.Count; i++)
            {
                if (runnings[i] == procname)
                {
                    runnings.RemoveAt(i);
                    ProcSync.procs[main] = runnings;
                    ProcSync.ProcCount--;
                    return true;
                }
            }
            

            return false;
        }

        public static int getProcCount(string main, string procname)
        {
            if (!ProcSync.procs.ContainsKey(main))
            {
                return 0;
            }
            int cnt = 0;
            List<string> runnings = (List<string>)ProcSync.procs[main];
            for (int i = 0; i < runnings.Count; i++)
            {
                if (runnings[i] == procname)
                {
                    cnt++;
                }
            }
            return cnt;
        }

        public static int getProcCount(string main)
        {
            if (!ProcSync.procs.ContainsValue(main))
            {
                return 0;
            }
            
            List<string> runnings = (List<string>)ProcSync.procs[main];
            return runnings.Count;
        }
        
    }
}
