import Vue from 'vue'
import Router from 'vue-router'
import PracticeList from '@/components/PracticeList'
import AppointmentStatus from '@/components/AppointmentStatus'
import BookAppointment from '@/components/BookAppointment'

Vue.use(Router)

export default new Router({
  routes: [{
      path: '/',
      name: 'List',
      component: PracticeList
    },
    {
      path: '/BookAppointment/:id',
      name: 'BookAppointment',
      component: BookAppointment
    },
    {
      path: '/status/:id',
      name: 'status',
      component: AppointmentStatus
    }
  ]
})
