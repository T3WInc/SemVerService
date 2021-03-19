cat /var/jenkins_home/my_password.txt | docker login -u schulzdl --password-VisualStudioVersion
docker tag semverservice schulzdl/semverservice:$1
docker push schulzdl/semverservice.api:$1