app_name='testconfig'

docker stop $app_name
docker remove $app_name
docker image remove $app_name
docker build -t $app_name .
docker run -it --name $app_name -p 5250:8080 $app_name -e "env=http://host.docker.internal:5250"