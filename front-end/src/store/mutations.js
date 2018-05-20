const mutations = {
  Initialize(state, bootdata) {
    state.Practices = bootdata.Practices
  },
  FeedReceived(state, item) {
    if (state.Feeds.filter(function (feed) {
        return feed.id == 12785623;
      }).length > 0) {
      console.log('skip it')
    } else {
      state.Feeds.push(item);
    }

  },
  SelectedPractice(state, item) {
    state.SelectedPractice = item;
  },
  AppointmentBooked(state, item) {
    state.QueueResonse = item;
  },
  SetPatientName(state, item) {
    state.PatientName = item;
  },
  ClearFeed(state) {
    state.Feeds = [];
  }
}

export default mutations;
