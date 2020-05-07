class NotificationHub {
    constructor(options, callback = (target) => { }) {
        this.connection = null;
        this.initialize();
        this.callback = callback;

        this.callback(this);
    }

    initialize() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
        this.connection.on("notificationResult", (data) => {
            this.notify(data);
        });
        this.connection.onclose(async () => { setTimeout(() => this.start(), 10000); });
        this.start();
    }

    start = async function () {
        var hub = this;
        await this.connection.start().then((e) => {
            console.log("notificationHub connected");
            hub.connection.invoke('getConnectionId').then(function (connectionId) {
                sessionStorage.setItem('conectionId', connectionId);// Send the connectionId to controller
            }).catch(err => console.error(err.toString()));;
        }).catch(err => {
            setTimeout(() => this.start(), 5000);
            return console.log(err);
        });
    }

    notify = async function (data) {
        if (data != null) {
            $.fn.alert(data)
        }
    }
}