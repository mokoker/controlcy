

    <script setup>

      import axios from "axios";
      import Table from './Table.vue'
      import { ref } from 'vue'

      const props = defineProps({
      valy: {
        type: String,
        required: true
      }
    })
    const status = ref("")
    const notFound=ref(false)
    const visy = ref(false)
    const tableData = ref()
    const fields = [
      'port','tcpUdp','status'
    ]


    function getx() {
      axios
      .get('http://localhost:5002/one/'+props.valy)
      .then((response) => {
        this.tableData = response.data.ports
        if (response.data.ports.length >0)
          {this.visy = true
          this.notFound = false
          this.status = `scan result for ${response.data.ipAddress} last scanned @ ${response.data.scanDate}`
        }
        else
          {this.notFound =true
            this.visy = false
          }
      }).catch((response)=>{
        if(response.response.status ==404)
        {
          this.notFound =true
          this.visy = false
        }
      })
    }
    </script>
<template>

        <div class="d-flex flex-column">
            <div class="p-2">  <h1 class="green d-flex justify-content-center">CONTROLCY</h1></div>
            <div class="input-group mb-3">
  <input type="text"  v-model="valy" class="form-control" placeholder="Ip or port to search" aria-label="Recipient's username" aria-describedby="button-addon2">
  <div class="input-group-append">
    <button  @click="getx()" class="btn btn-outline-secondary" type="button" id="button-addon2">Search</button>
  </div>

</div>         

 <!-- The table component -->
    
 <div class="table-responsive my-5" v-show=this.visy>
  <p>{{this.status}}</p>
<!-- The table component -->
 <Table :fields="fields" :tableData="tableData"></Table>
</div>
<div v-show=this.notFound>No open ports found</div>

  </div>


</template>



<style scoped>
    h1 {
      font-weight: 500;
      font-size: 3rem;
    }



</style>
    