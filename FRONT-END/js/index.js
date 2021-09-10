const SERVER_URL = "https://localhost:44338/api";
Vue.use(Toasted);
new Vue({
    el: "#app",
    data: () => ({
        User: {
            name: "",
            email: "",
            password: "",
        },
    }),
    methods: {
        async login() {
                console.log('hi');
            if (!this.User.email) {
                return this.$toasted.show("Please write email", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            if (!this.User.password) {
                return this.$toasted.show("Please write password", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            const payload = JSON.stringify(this.User);
            const url = SERVER_URL + "/User/authenticate";
            const r = await fetch(url, {
                method: "POST",
                body: payload,
                headers: {
                    "Content-type": "application/json",
                }
            });
            if(r.status == 200){
                const response = await r.json();
                if (response) {
                    this.$toasted.show("Cool!", {
                        position: "top-left",
                        duration: 1000,
                    });
              
                    
                localStorage.setItem('token', response.jwtToken);

                    window.location.href = "./get.html";
                } else {
                    this.$toasted.show("Something went wrong. Try again", {
                        position: "top-left",
                        duration: 1000,
                    });

                }
            }else{
                this.$toasted.show("Login not valid", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            
        }
    }
});