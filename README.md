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

Here some usefull commands:
• minikube start --driver=docker
• minikube start --driver=VMware (for VMWare virtualization)
• minikube config set driver docker
• minikube delete
• minikube status
• kubectl cluster-info
• minikube dashboard

If you are using Windows, Docker comes with kubectl, but on Linux it does not.

On Linux you can install kubectl, or you can run minikube kubectl instead of kubectl. 

I expect you to know how to use .NET and Docker.

## 1. Create a new Web API project
   
Create a project using whatever method you prefer naming, then edit the Program.cs.In this example ss you can see this application does very little. It returns a unique ID, the operating system, the time the application started, the current time, and the number of requests it has received.

Try running locally to make sure it works.

## 2. Create and build the Dockerfile service project

Here the steps:
• Add Docker Support to project in VS
• Build the Dockerfile
• Right click on project "Open in Terminal"

## 3. Start Minikube

Start Minikube using the following command:
• minikube start

Optionally, start the Minikube dashboard 
• minikube dashboard

## 4. Copy the Docker image to Minikube and push it to the hub

By default, Minikube and Kubernetes will try to pull images from a registry, so you need to copy the image created above to Minikube.

Do this with the following command:
• docker login
• docker build -t kraftcoding/minikubewebapiexamplenet8:dev .
• docker images
• docker push kraftcoding/minikubewebapiexamplenet8:dev
• docker pull kraftcoding/minikubewebapiexamplenet8:dev

## 5. Creating the pod

Create the file if doesnt exist. And execute:
• minikube image load kraftcoding/minikubewebapiexamplenet8:dev
• kubectl apply -f .\minikubewebapiexamplenet8-pod.yaml
• kubectl get pods

You should see something like this:
NAME         
      READY   STATUS    RESTARTS   AGE
webapiinminikube   1/1     Running   0 

## 6. Expose the pod
    8s 
Right now the pod is running, but it is not accessible from your local machine.

The simplest way to solve this is to port forward the pods it to your local machine.

• kubectl port-forward minikubewebapiexamplenet8 8080:8080

This will work for a single pod, not when we get to a deployment, but that is for another day.

## 7. Open the application

Now you can go to http://localhost:8080 and see the output from the application.

There you go, you should have a running ASP.NET Web API application running on Kubernetes using Minikube.
