# Code for my talk "Introduction to Kubernetes for Developers"

This repo contains the source code used for my talk "Introduction to Kubernetes for Developers". It contains both the webb app that is deployed to Kubernetes, and the different YAML files used to deploy it in various forms.

## Caveat

The configuration files are pulling an image from my private container registry. To get it to work for you, you need to build an image based on the Dockerfile in the _App directory, and push it to your own registry. After that, you need to set up a pull credential called `k8s4devs` by running something like this

```bash
kubectl create secret \
  docker-registry \
  regcred \
  --docker-server=<your-registry-server> \
  --docker-username=<your-name> \ 
  --docker-password=<your-pword> \ 
  --docker-email=<your-email>
```

You can read more about the process [here](https://kubernetes.io/docs/tasks/configure-pod-container/pull-image-private-registry/)!

---

Any questions? Feel free to give me a shout - [@ZeroKoll](https://twitter.com/zerokoll)