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

        public void removeFromAllGroups(string profilName)
        {
            List<string> groups = this.getAllGroups();
            foreach (string group in groups)
            {
                this.removeFromGroup(group, profilName);
            }
        }

        public Boolean updateGroupMember(string groupname, string profilNameOld, string profilNameNew)
        {
            List<string> member = getGroupMember(groupname);
            if (member.Contains(profilNameOld) && !member.Contains(profilNameNew))
            {
                member.Remove(profilNameOld);
                member.Add(profilNameNew);
                this.saveGroupMember(groupname, member);

                return true;
            }
            return false;
        }

        public void updateMemberInallGroups(string profilName, string newName)
        {
            List<string> groups = this.getAllGroups();
            foreach (string group in groups)
            {
                this.updateGroupMember(group, profilName, newName);
            }
        }

        public Boolean renameProfil(string oldName, string newName)
        {
            List<string> profiles = this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());
            if (profiles.Contains(oldName) && !profiles.Contains(newName))
            {
                this.Setup.setName(PConfig.KEY_PROFILS + "." + oldName, newName);
                this.updateMemberInallGroups(oldName, newName);
            }
            return false;
        }


        public int setPositionInGroup(string GroupName, string profileOne, string usePositionFrom)
        {
            List<string> member = this.getGroupMember(GroupName);
            if (member == null || member.Count < 2 || !member.Contains(profileOne) || !member.Contains(usePositionFrom))
            {
                return -1;
            }
            
            int posOrgin = member.IndexOf(usePositionFrom);
            member.Remove(profileOne);
            member.Insert(posOrgin, profileOne);
            this.saveGroupMember(GroupName, member);
            return posOrgin;
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

        public List<string> getGroupMember(string name)
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

        public List<string> getAllGroups()
        {
            return this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_NAMES, new List<string>());
        }

        private void saveAllGroups(List<string> groups)
        {
            this.Setup.setList(PConfig.KEY_GROUPS_NAMES, groups);
        }
    }
}
