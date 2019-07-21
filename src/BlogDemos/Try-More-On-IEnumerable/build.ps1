docker build -t 192.168.2.150:5001/justin/myconsole -f server.Dockerfile .
docker push 192.168.2.150:5001/justin/myconsole
docker rmi 192.168.2.150:5001/justin/myconsole

docker build -t 192.168.2.150:5001/justin/myconsoleclient -f client.Dockerfile .
docker push 192.168.2.150:5001/justin/myconsoleclient
docker rmi 192.168.2.150:5001/justin/myconsoleclient

docker rm $(docker ps -a -q)
docker rmi $(docker images --filter dangling=true -q --no-trunc)