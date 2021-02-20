import pygit2
import requests

def getTip(repo):
    result = ""
    head = repo.head
    parts = head.name.split('/')
    if (len(parts) == 4) :
        result = parts[2]+'/'+parts[3]
    result = parts[2]+'/'+parts[3]
    return result

def main():
    repo = pygit2.Repository('/home/ubuntu/repo/semverservicesample/.git')
    branch = getTip(repo)

    print(branch)

if __name__ == "__main__":
    main()
