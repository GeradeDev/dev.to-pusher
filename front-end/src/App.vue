<template>
  <div id="app">
    <div style="margin-bottom:10px">
      <Navigation/>
    </div>
    <router-view/>
  </div>
</template>

<script>
import Navigation from '@/components/Navigation'
export default {
  name: 'App',
  components: { Navigation }
}
</script>

<style>

</style>
