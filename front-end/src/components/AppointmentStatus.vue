<template>
    <div class="container">
        <div class="card" style="background-color:#3273dc;color:white!important;">
            <div class="card-image">
                <ul>
                    <li style="font-size:24px;">
                        <span id="hours" style="font-size:36px;">{{queueResonse.Ticket}}</span>
                        Ticket Number
                    </li>
                </ul>
            </div>

            <div class="card-content">
                <div class="content text-center">
                    <p class="subtitle is-6 title-subtext">Please be on time.</p>
                    <p class="subtitle is-6 title-subtext">See you and {{patientName}} soon.</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
    name: "Status",
    computed: {
        ...mapGetters({
            queueResonse: 'queueResonse',
            patientName: 'patientName'
        }),
    },
    mounted() {
        if (this.queueResonse == null) {
            this.$router.push({ name: 'List' })
        }

    }
}
</script>

<style scoped>
.title-subtext {
  color: white;
}
.card-image {
  text-align: center;
}
.text-center {
  text-align: center;
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

