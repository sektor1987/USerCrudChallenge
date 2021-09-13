const SERVER_URL = "https://twamtcbyw5.execute-api.us-east-2.amazonaws.com/api";
Vue.use(Toasted);
Vue.filter("currency", value => {
    return "$".concat(parseFloat(value).toFixed(2));
});
new Vue({
    el: "#app",
    data: () => ({
        Users: [],
    }),
      // definir métodos bajo el objeto `methods`
      // para este caso no utilizaremos axios (es un crud simple :P)
    methods: {
        async getUsers() {
            const url = SERVER_URL + "/User/GetUsers";
                const r = await fetch(url, { 
                    headers: new Headers({
                      'Authorization': 'Bearer '+localStorage.getItem('token')
                    }), 
                  });
         
            const Users = await r.json();
            this.Users = Users;
        },
        edit(User) {
            window.location.href = "./edit.html?id=" + User.id;
        },
        async deleteUser(User) {
            const result = await Swal.fire({
                title: 'Delete',
                text: "Are you sure you want to delete this User?",
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#1f9bcf',
                cancelButtonColor: '#d9534f',
                cancelButtonText: 'No',
                confirmButtonText: 'Yes, delete it'
            });
            // Stop if user did not confirm
            if (!result.value) {
                return;
            }
            const r = await fetch(SERVER_URL + "/User/" + User.id, {
                method: "DELETE",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer '+localStorage.getItem('token')
                }
            });
            //const response = await r.json();
            if (r.ok == true) {
                this.$toasted.show("Deleted", {
                    position: "top-left",
                    duration: 1000,
                });
                await this.getUsers();
            } else {
                this.$toasted.show("Something went wrong. Try again", {
                    position: "top-left",
                    duration: 1000,
                });
            }
        }
    },
    async mounted() {
        await this.getUsers();
    }
});