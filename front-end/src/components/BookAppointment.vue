<template>
    <div class="container">
        <div style="padding-bottom:20px;">
            <div class="card" style="border:1px solid #3273dc;">
                <div style="font-size:20px;text-align:center"> Madera Animal Hopital</div>
                <div style="font-size:18px;text-align:center"> Current Wait time</div>
                <div class="card-image">
                    <ul>
                        <li v-if="feeds.length>0">
                            <span id="hours">{{hours}}</span>Hour</li>
                        <li v-if="feeds.length<=0">
                            <span id="hours">0</span>Hour</li>
                        <li v-if="feeds.length>0">
                            <span id="minutes">{{minutes}}</span>Minutes</li>
                        <li v-if="feeds.length<=0">
                            <span id="minutes">0</span>Minutes</li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="columns">
            <div class="column" style="margin-top:40px;">
                <div v-for="(item, x) in feeds" style="border:1px solid #3273dc;padding:10px;" v-if="feeds.length>0" v-bind:key="x">
                    <article class="media">
                        <figure class="media-left">
                            <p class="image is-128x128">
                                <img v-bind:src="getImageUrl(x)">
                            </p>
                        </figure>
                        <div class="media-content">
                            <ul>
                                <li>
                                    <div id="hours" style="font-size:24px;">{{item.TicketNumber}}</div>
                                    <div id="hours" style="font-size:20px;">{{item.FirstName}} {{item.LastName}}</div>
                                </li>
                            </ul>
                        </div>
                    </article>
                </div>
                <div v-if="feeds.length<=0" style="border:1px solid #3273dc; padding:20px;">
                    <div>No one is in queue.</div>
                </div>
            </div>
            <div class="column" style="margin:10px;">
                <form v-on:submit.prevent="bookAppointment">
                    <div class="field">
                        <label class="label">First Name</label>
                        <div class="control">
                            <input class="input" v-model="FirstName" type="text" placeholder="First Name">
                            <span class="error" v-if="errors.indexOf('InvalidFirstName')>-1">First Name is required. </span>
                        </div>
                    </div>
                    <div class="field">
                        <label class="label">Last Name</label>
                        <div class="control">
                            <input class="input" v-model="LastName" type="text" placeholder="Last Name">
                            <span class="error" v-if="errors.indexOf('InvalidLastName')>-1">Last Name is required. </span>
                        </div>
                    </div>
                    <div class="field">
                        <label class="label">Email Address</label>
                        <div class="control">
                            <input class="input" v-model="EmailAddress" type="text" placeholder="Email Address">
                            <span class="error" v-if="errors.indexOf('InvalidEmailAddress')>-1">EmailAddress is required. </span>
                        </div>
                    </div>
                    <div class="field">
                        <label class="label">Patient Name</label>
                        <div class="control">
                            <input class="input" v-model="PatientName" type="text" placeholder="Patient Name">
                            <span class="error" v-if="errors.indexOf('InvalidPatientName')>-1">PatientName is required. </span>
                        </div>
                    </div>

                    <div class="field">
                        <label class="label">Visit Reason</label>
                        <div class="control">
                            <textarea class="textarea" v-model="Description" placeholder="Visit Reason"></textarea>
                            <span class="error" v-if="errors.indexOf('InvalidDescription')>-1">Description is required. </span>
                        </div>
                    </div>

                    <div class="field">
                        <div class="control">
                            <label class="checkbox">
                                <input type="checkbox" v-model="checked"> I agree to the
                                <a href="#">terms and conditions</a>
                            </label>
                        </div>
                    </div>

                    <div class="field is-grouped">
                        <div class="control">
                            <button type="submit" class="button is-link" :class="{'is-loading':Loading}" v-bind:disabled="!checked">Book Appointment</button>
                        </div>
                        <div class="control">
                            <button class="button is-text">Cancel</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
import PusherClient from '../api/pusherClient'
export default {
    name: "BookAppointment",
    computed: {
        ...mapGetters({
            feeds: 'feeds',
            selectedPractice: 'selectedPractice'
        }),
    },
    data() {
        return {
            EmailAddress: null,
            FirstName: null,
            LastName: null,
            Description: null,
            PatientName: null,
            checked: false,
            hours: 0,
            minutes: 0,
            Loading: false,
            errors: []
        }
    },
    created() {
        console.log(this.$route.params.id);
    },
    mounted() {
        this.$store.dispatch('ClearFeed');
        window.document.title = "Current Wait time"
        var paramId = this.$route.params.id;
        var feedId = `Feed-${paramId}`
        this.hospitalFeed = new PusherClient(feedId);
        this.hospitalFeed.subscribe((item) => {
            var waitItem = JSON.parse(item.data);
            waitItem.id = item.id;
            console.log(waitItem.id);
            this.$store.dispatch('FeedReceived', waitItem);
        });
        var waitTimefeedId = `WaitTimeFeed-${paramId}`
        this.hospitalTimeFeed = new PusherClient(waitTimefeedId);
        this.hospitalTimeFeed.subscribe((item) => {
            var waitItem = JSON.parse(item.data);
            this.hours = Math.floor(waitItem.WaitTime / 60);
            this.minutes = waitItem.WaitTime % 60;
            console.log(waitItem);
        });
    },
    methods: {
        getImageUrl: function (id) {
            return "http://i.pravatar.cc/150?img=" + id
        },
        bookAppointment: function () {
            this.errors = [];
            if (!this.FirstName || this.FirstName == '') {
                this.errors.push("InvalidFirstName");
            }
            if (!this.LastName || this.LastName == '') {
                this.errors.push("InvalidLastName");
            }
            if (!this.EmailAddress || this.EmailAddress == '') {
                this.errors.push("InvalidEmailAddress");
            }
            if (!this.Description || this.Description == '') {
                this.errors.push("InvalidDescription");
            }
            if (!this.PatientName || this.PatientName == '') {
                this.errors.push("InvalidPatientName");
            }

            if (this.errors.length > 0) {
                return;
            }

            var data = {
                "PracticeId": this.$route.params.id,
                "PatientName": this.PatientName,
                "Description": this.Description,
                "Type": "W",
                "User": {
                    "UserId": this.EmailAddress,
                    "FirstName": this.FirstName,
                    "LastName": this.LastName,
                    "EmailAddress": this.EmailAddress
                }
            };
            this.Loading = true;
            this.$store.dispatch('SetPatientName', this.PatientName);
            this.$store.dispatch('BookAppointment', data)
                .then((response) => {
                    this.Loading = false;
                    this.$store.dispatch('AppointmentBooked', response.data);
                    this.$router.push({ name: 'status', params: { id: response.data.Ticket } })
                }, (error) => { console.log(error); this.Loading = false; });
        }
    }
}
</script>
<style scoped>
.card-image {
  text-align: center;
}
.error {
  color: #ff3860;
}
.card-image li {
  display: inline-block;
  font-size: 1.5em;
  list-style-type: none;
  padding: 10px;
  text-transform: uppercase;
}

.card-image li span {
  display: block;
  font-size: 3.5rem;
}

@media only screen and (max-width: 480px) {
  .card-image li {
    font-size: 1.5em;
  }

  .card-image li span {
    font-size: 3.5rem;
  }
}
</style>
