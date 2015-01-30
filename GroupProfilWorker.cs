using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class GroupProfilWorker
    {
        private PConfig Setup;
        public GroupProfilWorker(PConfig usedConf)
        {
            this.Setup = usedConf;
        }

        public Boolean joinToNewGoup(string nameOne, string nameTwo)
        {
            if (nameTwo == nameOne)
            {
                return false;
            }
            List<string> groups = this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_NAMES, new List<string>());
            List<string> member = new List<string>();
            member.Add(nameOne);
            member.Add(nameTwo);
            int randomNr = 1;
            string newName = "New Group";
            while (groups.Contains(newName))
            {
                newName = "New Group" + randomNr;
                randomNr++;
            }

            groups.Add(newName);
            this.Setup.setList(PConfig.KEY_GROUPS_NAMES, groups);
            this.Setup.setList(PConfig.KEY_GROUPS_MEMBERS + "." + newName, member);
            return true;
        }


        public Boolean joinToExistingGoup(string groupName, string profilName)
        {

            List<string> groups = this.getAllGroups();
            if (groups.Contains(groupName))
            {
                List<string> member = this.getGroupMember(groupName);
                if (!member.Contains(profilName))
                {
                    member.Add(profilName);
                    this.saveGroupMember(groupName, member);
                    return true;
                }
            }
            return false;
        }


        public Boolean removeFromGroup(string groupname, string profilName)
        {
            List<string> member = getGroupMember(groupname);
            if (member.Contains(profilName))
            {
                member.Remove(profilName);
                this.saveGroupMember(groupname,member);

                return true;
            }
            return false;
        }


        public Boolean renameGroup(string oldName, string newname)
        {
            if (oldName == newname)
            {
                return false;
            }

            List<string> groups = this.getAllGroups();
            if (groups.Contains(oldName) && !groups.Contains(newname))
            {
                List<string> member = getGroupMember(oldName);
                this.saveGroupMember(newname, member);
                this.saveGroupMember(oldName, null);
                groups.Remove(oldName);
                groups.Add(newname);
                this.saveAllGroups(groups);
                return true;
            }

            return false;
        }

        private List<string> getGroupMember(string name)
        {
            return this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_MEMBERS + "." + name, new List<string>());
        }

        private void saveGroupMember(string name, List<string> member)
        {
            if (member != null && member.Count() > 0)
                this.Setup.setList(PConfig.KEY_GROUPS_MEMBERS + "." + name, member);
            else
            {
                this.Setup.setList(PConfig.KEY_GROUPS_MEMBERS + "." + name, null);
                List<string> groups = this.getAllGroups();
                if (groups.Contains(name))
                {
                    groups.Remove(name);
                    this.saveAllGroups(groups);
                }
                
            }

        }

        private List<string> getAllGroups()
        {
            return this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_NAMES, new List<string>());
        }

        private void saveAllGroups(List<string> groups)
        {
            this.Setup.setList(PConfig.KEY_GROUPS_NAMES, groups);
        }
    }
}
