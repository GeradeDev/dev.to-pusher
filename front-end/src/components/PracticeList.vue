<template>
  <div class="container">
    <!-- <div v-for="(item, x) in practices" v-bind:key="x" style="border:solid red 1px;">
      {{item.Name}} {{item.CurrentWaitTime}}
     
    </div> -->
    <div class="card" v-for="(item, counter) in practices" v-bind:key="counter">
      <div class="card-content">
        <div class="media">
          <div class="media-left">
            <figure class="image is-128x128">
              <img src="https://bulma.io/images/placeholders/128x128.png" alt="Placeholder image">
            </figure>
          </div>
          <div class="media-content">
            <p class="title is-4"> {{item.Name}}</p>
            <p class="subtitle is-6">{{item.Address}}, {{item.State}} {{item.Zip}}</p>
            <a class="button is-link" @click="GetInLine(item)">Get In line</a>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>

import { mapGetters } from 'vuex'

export default {
  name: 'PracticeList',
  computed: {
    ...mapGetters({
      practices: 'practices',
    }),
  },
  data() {
    return {
      VetInfo: [],
    }
  },
  methods: {
    GetInLine(selectedPractice) {
      console.log(selectedPractice);
      this.$store.dispatch('SelectedPractice', selectedPractice);
      this.$router.push({ name: 'BookAppointment', params: { id: selectedPractice.PracticeId } })
    }
  }

}
</script>
