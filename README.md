# Starting up
Navigate to LePicka folder (where dockerfiles are) and run   (mind the dot at the end of the command)
- `docker build -t lepicka-auth -f .\DockerfileAuth .`
- `docker build -t lepicka-products -f .\DockerfileProducts .`

If you select different name than **lepicka-auth and/or lepicka-products** you will have to change names in k8s files ([auth-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/auth-depl.yaml) and [products-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/products-depl.yaml)) in spec/template/spec/containers/image and assign there the name of created image

Database password fetched from kubernetes secrets. Run this command to create kubernetes secret with password. If you use different name than SA_PASSWORD or different password than Password1! you will have to change it also in [mssql-auth-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/mssql-auth-depl.yaml), [mssql-prod-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/mssql-prod-depl.yaml) and appsettings.json of projects - [Products](https://github.com/4190/LePicka/blob/master/LePicka/LePickaProducts.API/appsettings.json), [Auth](https://github.com/4190/LePicka/blob/master/LePicka/Auth/appsettings.json)
- `kubectl create secret generic mssql --from-literal=SA_PASSWORD="Password1!"`


Then navigate to K8S folder and run: 
- `kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.0/deploy/static/provider/cloud/deploy.yaml  `  (needed for ingress nginx gateway and load balancer)
- `kubectl apply -f .  `   (to apply all files in folder)

Or apply yaml files separately
- `kubectl apply -f .\rabbitmq-depl.yaml`
- `kubectl apply -f .\local-pvc.yaml`
- `kubectl apply -f .\local-pvc-prod.yaml`
- `kubectl apply -f .\mssql-auth-depl.yaml`
- `kubectl apply -f .\mssql-prod-depl.yaml`
- `kubectl apply -f .\products-depl.yaml`
- `kubectl apply -f .\auth-depl.yaml`

# Changed something in code?
If you changed something in code of one of projects rebuild it's docker image with one of commands from Starting Up for respective service. After the docker image is rebuilt run
`kubectl rollout restart deployment <name of deployment>`  Use name of deployment that is using rebuilt docker image

Run `kubectl get deployments` to see all existing deployments. Check deployment files to make sure which uses the image

# Swagger
You can enable or disable swagger when deployed in kubernetes by changing it in appsettings.json in container files in app folder or you can change it in project and rebuild image - [Auth](https://github.com/4190/LePicka/blob/master/LePicka/Auth/appsettings.json), [Products](https://github.com/4190/LePicka/blob/master/LePicka/LePickaProducts.API/appsettings.json)
