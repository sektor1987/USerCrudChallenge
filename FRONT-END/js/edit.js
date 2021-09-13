const SERVER_URL = "https://twamtcbyw5.execute-api.us-east-2.amazonaws.com/api";
Vue.use(Toasted);
new Vue({
    el: "#app",
    data: () => ({
        user: {
            id: "",
            name: "",
            price: "",
            rate: "",
        },
    }),
    async mounted() {
        await this.getuserDetails();
    },
    methods: {
        async getuserDetails() {
            const urlSearchParams = new URLSearchParams(window.location.search);
            const id = urlSearchParams.get("id");

              
            const r = await fetch(`${SERVER_URL}/User/${id}`, { 
                headers: new Headers({
                  'Authorization': 'Bearer '+localStorage.getItem('token')
                }), 
              });
            const user = await r.json();
            this.user = {
                id: user.id,
                name: user.name,
                email: user.email,
                password: user.password,
            };
        },
        async save() {
            if (!this.user.name) {
                return this.$toasted.show("Please write name", {
                    position: "top-left",
                    duration: 1000,
                });
            }

            if (!this.user.email) {
                return this.$toasted.show("Please write email", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            if (!this.user.password) {
                return this.$toasted.show("Please write password", {
                    position: "top-left",
                    duration: 1000,
                });
            }
            const payload = JSON.stringify(this.user);
            const url = SERVER_URL + "/user";
            const r = await fetch(url, {
                method: "PUT",
                body: payload,
                headers: {
                    "Content-type": "application/json",
                    'Authorization': 'Bearer '+localStorage.getItem('token')
                }
            });
            const response = await r.json();
            if (response) {
                window.location.href = "./get.html";
            } else {
                this.$toasted.show("Something went wrong. Try again", {
                    position: "top-left",
                    duration: 1000,
                });
            }
        }
    }
});