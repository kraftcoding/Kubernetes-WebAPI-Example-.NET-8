# Minikube WebAPI Example .NET-8

Getting started with Kubernetes is not easy, it feels like there is so much to understand and build to do even the simplest thing. Because of this I often think
Kubernetes is being used in scenarios where simpler solutions would be both more appropriate and easier to implement. But this blog post is not about that.
This post is also not going to explain what Kubernetes is, or how the pods, deployments, services, etc, relate - there are plenty of very good resources on that.
Instead, this post will show you, step by step, how to get a simple ASP.NET Web API application running on Kubernetes using Minikube. 
Kubernetes locally, it works on Windows, Linux, and Mac.

At the end, you will have a single instance of the application running in a pod, and it will be accessible from your local machine.

## Prerequisites

To follow along with this post you need to install a few tools.
• Docker
• MiniKube


If you are using Windows, Docker comes with kubectl, but on Linux it does not.

On Linux you can install kubectl, or you can run minikube kubectl instead of kubectl. 

I expect you to know how to use .NET and Docker.

## 1. Create a new Web API project
   
Create a project using whatever method you prefer naming it WebApiInMinikube, then change the Program.cs file to look like this 
 1var builder = WebApplication.CreateBuilder(args);
 2
 3string id = Guid.NewGuid().ToString();
 4string osDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
 5DateTime startUpTime = DateTime.Now;
 6int requestCount = 0;
 7
 8var app = builder.Build();
 9
https://
10app.MapGet("/", () => $"Id {id}\nOperating System {osDescription}\nStarted at {startUpTime}\n\nCurrent time {DateTime.Now}\nRequest count {++requestCount}");
11
12app.Run();
As you can see this application does very little. It returns a unique ID, the operating system, the time the application started, the current time, and the number of
requests it has received.
Try running locally to make sure it works.
3. Create a Dockerfile
This post is not about Docker, or how to use it. Here is the Dockerfile, it is more or less a copy of what Visual Studio would create.
## # This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApiInMinikube.csproj", "."]
RUN dotnet restore "./WebApiInMinikube.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./WebApiInMinikube.csproj" -c $BUILD_CONFIGURATION -o /app/build
### This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApiInMinikube.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
### This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiInMinikube.dll"]
## 3. Build and run the Docker image
Build the Docker image using the following command 
docker build -t webapiinminikube:1.0 .
Test that it works by running the container 
docker run -it --rm -p 8080:8080 webapiinminikube:1.0
You should be able to navigate to http://localhost:8080 and see the output from the application. Stop the container.
## 4. Start Minikube
Start Minikube using the following command 
minikube start
Optionally, start the Minikube dashboard 
minikube dashboard
## 5. Copy the Docker image to Minikube
By default, Minikube and Kubernetes will try to pull images from a registry, so you need to copy the image created above to Minikube.
Do this with the following command 
minikube image load webapiinminikube:1.0
If if works you will get the wonderful output of nothing, a blank line! Very helpful!
## 6. Creating the pod
Create a file called webapiinminikube-pod.yaml with the following content 
apiVersion: v1
kind: Pod
metadata:
name: webapiinminikube
spec:
containers:
  - 
name: webapiinminikube
image: webapiinminikube:1.0
ports:
    - containerPort: 8080
Because this is yaml, the indentation is important, if copy/pasting is not working for you download the full source code and copy the file from there.
Note how the image name is the same as the one you copied to Minikube.
Start the pod using the following command 
kubectl apply -f webapiinminikube-pod.yaml
You can check that the pod is running using the following command 
kubectl get pods
You should see something like this 
NAME         
      READY   STATUS    RESTARTS   AGE
webapiinminikube   1/1     Running   0      
## 7. Expose the pod
    8s 
Right now the pod is running, but it is not accessible from your local machine.
The simplest way to solve this is to port forward the pods it to your local machine.
kubectl port-forward webapiinminikube 8080:8080
This will work for a single pod, not when we get to a deployment, but that is for another day.
8. Open the application
Now you can go to http://localhost:8080 and see the output from the application.
There you go, you should have a running ASP.NET Web API application running on Kubernetes using Minikube.
