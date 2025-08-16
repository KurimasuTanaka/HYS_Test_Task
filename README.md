# Test project by Vedmitskyi Stanislav for HYS Enterprise Back-end Course #
Sorry for the delay — I only found the email in my spam folder yesterday. Is there still a chance that my test project will be reviewed?

## Runnig instructions: ##
1) Clone repository to your local machine: git clone ` https://github.com/KurimasuTanaka/HYS_Test_Task `
2) Build docker image: ` docker build -t hys-test-app `
3) Run docker container: ` docker run -d -p 5000:5000 --name hys-test-container hys-test-app `

## Faced issues: ##
I’m not entirely sure if I understood the requirement “Timezone is UTC.” Should I handle it explicitly, for example by converting from the local time to UTC?
