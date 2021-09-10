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
        async save() {
            if (!this.User.name) {
                return this.$toasted.show("Please write name", {
                    position: "top-left",
                    duration: 1000,
                });
            }

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
            const url = SERVER_URL + "/User";
            const r = await fetch(url, {
                method: "POST",
                body: payload,
                headers: {
                    "Content-type": "application/json",
                    'Authorization': 'Bearer '+localStorage.getItem('token')
                }
            });
            const response = await r.json();
            if (response) {
                this.$toasted.show("Saved", {
                    position: "top-left",
                    duration: 1000,
                });
                this.User = {
                    name: "",
                    price: null,
                    rate: null,
                };
            } else {
                this.$toasted.show("Something went wrong. Try again", {
                    position: "top-left",
                    duration: 1000,
                });
            }
        }
    }
});