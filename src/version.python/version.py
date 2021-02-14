import pygit2
import requests

def getTips(branches):
    result = ""
    for branch in branches :
        if (branch.Tip != null) :
            if (branch.Tip.IsHead) :
                if (result == "") :
                    result = branch.Name
                else :
                    result = result + "," + branch.Name

    if (result == "") :
        result = backup

    return result.split(',')




repo = pygit2.Repository('/home/ubuntu/repo/semverservicesample/.git')

branches = repo.branches.remote
print(getTips(branches))