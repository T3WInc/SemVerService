#!/usr/bin/python

import pygit2
import requests
import sys

key="d4756207-07ac-4ffa-beb0-3149ebe0fe19"
product="semverservicesample"

def getTip(repo):
    result = ""
    head = repo.head
    parts = head.name.split('/')
    if (len(parts) == 4) :
        result = parts[2]+'/'+parts[3]
    #result = parts[2]+'/'+parts[3]
    return result

def sendToServer(branch):
    url = 'https://localhost:5001/api/Version/'+ key
    payload = {'Product': product, 'Branch': branch}
    r = requests.get(url, params=payload, verify=False)
    
    #print(r.url)
    #print(r.content)
    return r.content

def main():
    print ('Total number of arguments:', format(len(sys.argv)))
    print ('Argument List:', str(sys.argv))

    repo = pygit2.Repository('/home/ubuntu/repo/semverservicesample/.git')
    branch = getTip(repo)

    #print(branch)
    sys.argv[3]=(sendToServer(branch))
    print ('Version Number Is:', str(sys.argv[3]))

if __name__ == "__main__":
    main()
