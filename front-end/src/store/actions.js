import AppointmentRequest from '../api/AppointmentRequest'

const actions = {
    Initialize: ({commit }, bootdata) => {
       commit('Initialize', bootdata)
    },
    FeedReceived:({commit}, feed)=>{
      commit('FeedReceived', feed)
    },
    SelectedPractice:({commit}, data)=>{
      console.log(data)
      commit('SelectedPractice', data)
    },
    BookAppointment:({commit}, appointment)=>{
      return new Promise((resolve, reject) => {
        var requst = new AppointmentRequest();
        requst.bookAppointment(appointment)
        .then((data)=>{
          resolve(data);
        }, ()=>{
          reject("failed")
        })
      });
    },
    AppointmentBooked:({commit}, appointment)=>{
      console.log(appointment);
      commit('AppointmentBooked', appointment);
    },
    SetPatientName:({commit}, patientName)=>{
      commit('SetPatientName', patientName);
    },
    ClearFeed:({commit}, patientName)=>{
      commit('ClearFeed', patientName);
    },
}

export default actions;
