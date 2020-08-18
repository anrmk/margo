class NotificationHub {
    constructor(options, callback = (target) => { }) {
        this.options = options;
        this.connection = null;
        this.initialize();
        this.callback = callback;

        this.callback(this);
    }

    initialize() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();

        this.connection.on("receiveMessage", (data) => {
            $.fn.message(data);
        });

        this.connection.on("signout", (data) => {
            $('body').dimmer({
                    displayLoader: true,
                    loaderVariation: 'slow orange medium elastic',
                    loaderText: ' Your account has been desabled, please contact your System Administrator!'
                })
                .dimmer('show')
            //$.post('/account/logout', );
        });

        //this.connection.connectionClosed(async () => { setTimeout(() => this.start(), 10000); });
        this.connection.onclose(async () => { setTimeout(() => this.start(), 10000); });

        this.start();
    }

    start = function () {
        var hub = this;
        try {
            this.connection.start().then((e) => {
                console.log("notificationHub connected");
                hub.connection.invoke('getConnectionId').then((connectionId) => {
                    sessionStorage.setItem('conectionId', connectionId);// Send the connectionId to controller
                }).catch(err => console.error(err.toString()));
            }).catch(err => {
                setTimeout(() => this.start(), 5000);
                return console.log(err);
            });
        } catch (e) {
            console.log(e);
        }
    }

    notify = function (data) {
        if (data != null) {
            $.fn.alert(data)
        }
    }
}