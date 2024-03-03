Navigate to LePicka folder (where dockerfiles are) and run   (mind the dot at the end of the command)
- docker build -t lepicka-auth -f .\DockerfileAuth .
- docker build -t lepicka-products -f .\DockerfileProducts .

If you select different name than **lepicka-auth and/or lepicka-products** you will have to change names in k8s files ([auth-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/auth-depl.yaml) and [products-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/products-depl.yaml)) in spec/template/spec/containers/image and assign there the name of created image

Database password fetched from kubernetes secrets. Run this command to create kubernetes secret with password. If you use different name than SA-PASSWORD or different password than Password1! you will have to change it also in [mssql-auth-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/mssql-auth-depl.yaml) and [mssql-prod-depl.yaml](https://github.com/4190/LePicka/blob/master/K8S/mssql-prod-depl.yaml)
-kubectl create secret generic mssql --from-literal=SA_PASSWORD="Password1!"


Then navigate to K8S folder and run: 
- kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.0/deploy/static/provider/cloud/deploy.yaml    (needed for ingress nginx gateway and load balancer)
- kubectl apply -f .     (to apply all files in folder)

Or apply yaml files separately
- kubectl apply -f .\rabbitmq-depl.yaml
- kubectl apply -f .\local-pvc.yaml
- kubectl apply -f .\local-pvc-prod.yaml
- kubectl apply -f .\mssql-auth-depl.yaml
- kubectl apply -f .\mssql-prod-depl.yaml
- kubectl apply -f .\products-depl.yaml
- kubectl apply -f .\auth-depl.yaml
