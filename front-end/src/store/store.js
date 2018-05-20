import Vue from 'vue'
import Vuex from 'vuex'
import actions from './actions'
import mutations from './mutations'
import * as getters from './getters'

Vue.use(Vuex)

const state = {
  Practices: [],
  SelectedPractice:null,
  Feeds: [],
  QueueResonse:null,
  PatientName:null,
}

const store = new Vuex.Store({
  debug: true,
  state: state,
  getters: getters,
  mutations: mutations,
  actions: actions
})


export default store;
