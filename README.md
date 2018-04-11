## BIAI-  Path Finder
---
#Create a branch and switch:
git checkout -b <new_branch_name>
---
#Push branch:
git push -u origin <branch_name>
---
#Merge branch to remote:
git checkout master
git pull origin master
git merge <branch_name>
git push origin master