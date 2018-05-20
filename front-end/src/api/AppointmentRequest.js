import axios from 'axios';

export default class AppointmentRequest {
  constructor() {
    this.baseUrl = 'https://t6w22b5mi7.execute-api.us-east-1.amazonaws.com/prod/back-end-dev-appointment';
  }

  bookAppointment(data) {
    return axios.post(this.baseUrl, data);
  }
}