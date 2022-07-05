<template>
  <div>
    <v-card class="mx-auto" max-width="1000" outlined>
        <v-card-title>
            {{ t("autotransferschedule") }}
            <v-spacer />
            <v-chip :color="values.schedule.status === 'running' ? 'success' : 'error' " class="mr-2" dark outlined>
                {{ t("status") }} : {{ values.schedule.status }}
            </v-chip>
            <v-chip v-show="values.schedule.lastRunDate" color="primary" dark outlined>
                {{ t("lastrun") }} : {{ values.schedule.lastRunDate }}
            </v-chip>
        </v-card-title>
        <v-card-text>
            <v-text-field
                v-model="values.schedule.type"
                prepend-icon="far fa-calendar-alt"
                dense
                outlined
                readonly
            />
            <v-text-field
                v-model="values.schedule.time"
                :label="t('time')"
                prepend-icon="far fa-clock"
                clearable
                outlined
                dense
                hide-details
            />
        </v-card-text>
        <v-card-actions class="d-block text-right">
            <v-btn color="success" @click="setSchedule" :disabled="!values.schedule.type || !values.schedule.time">{{ t("save") }}</v-btn>
            <v-btn class="button is-danger"
                v-show="values.schedule.status === 'running'"
                color="error"
                @click="stopSchedule">
                {{ t("stop") }}
            </v-btn>

        </v-card-actions>
    </v-card>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import appStore from '@/store/app'

import moment from 'moment'
import zohoScheduleApi from '@/api/resources/zohoschedule'

// extract global import function
const { t } = useI18n()
const app = appStore()

// define variables
const time = ref(null) // Note: delete if it has not used
const isShowTime = ref(false) // Note: delete if it has not used
const types = reactive([
  { text: "time", value: "TIME" }
])
const values = reactive({
  schedule: {
    type: "TIME",
    time: "12:00",
    lastRunDate: null,
    status: null
  }
})

// handle methods
const convertLastRunToLocalTime = () => {
  values.schedule.lastRunDate = moment(moment
        .utc(values.schedule.lastRunDate)
        .toDate())
      .local()
      .format("LLLL");
}

async function setSchedule() {
  await app.load(async () => {
    const schedule = values.schedule;
    schedule.TimeZoneOffSet = (new Date()).getTimezoneOffset().toString();
    schedule.Time = values.schedule.time;
    schedule.Type = values.schedule.type;

    values.schedule = await zohoScheduleApi.start(schedule);
    convertLastRunToLocalTime();
  })
  
  app.success(t("schedulesave"), 5000);
}

async function stopSchedule() {
  await app.load(async () => {
    values.schedule = await zohoScheduleApi.stop();
    convertLastRunToLocalTime();
    
    if (!values.schedule.type) {
      values.schedule.type = "TIME";
    }
    if (!values.schedule.time) {
      values.schedule.time = "12:00";
    }
  })
  
  app.success(t("schedulestop"), 5000);
}

onMounted(async () => {
  await app.load(async () => {
    values.schedule = await zohoScheduleApi.get()
    convertLastRunToLocalTime()
  })
})

</script>


<!--<script>-->
<!--import moment from "moment"-->
<!--import zohoScheduleApi from '@/api/resources/zohoschedule'-->
<!--import appStore from '@/store/app.js'-->
<!--import { defineComponent } from 'vue'-->
<!--import { useI18n } from 'vue-i18n'-->

<!--const app = appStore()-->

<!--export default defineComponent({-->
<!--    setup() {-->
<!--      const { t } = useI18n()-->
<!--      return { t }-->
<!--    },-->
<!--    data() {-->
<!--        return {-->
<!--            time: null,-->
<!--            isShowTime: false,-->
<!--            types: [-->
<!--                { text: "time", value: "TIME" }-->
<!--            ],-->
<!--            values: {-->
<!--                schedule: {-->
<!--                    type: "TIME",-->
<!--                    time: "12:00",-->
<!--                    lastRunDate: null,-->
<!--                    status: null-->
<!--                }-->
<!--            }-->
<!--        };-->
<!--    },-->
<!--    async created() {-->
<!--        await app.load(async () => {-->
<!--          this.values.schedule = await zohoScheduleApi.get();-->
<!--          this.convertLastRunToLocalTime();-->
<!--        })-->
<!--    },-->
<!--    methods: {-->
<!--        convertLastRunToLocalTime() {-->
<!--            const lastRun = this.values.schedule.lastRunDate;-->
<!--            this.values.schedule.lastRunDate = moment(moment.utc(lastRun).toDate()).local().format("LLLL");-->
<!--        },-->
<!--        async setSchedule() {-->
<!--            let time = "";-->
<!--            const type = this.values.schedule.type;-->
<!--            time = this.values.schedule.time;-->

<!--            await app.load(async () => {-->
<!--                const schedule = this.values.schedule;-->
<!--                schedule.TimeZoneOffSet = (new Date()).getTimezoneOffset().toString();-->
<!--                schedule.Time = time;-->
<!--                schedule.Type = type;-->
<!--                -->
<!--                this.values.schedule = await zohoScheduleApi.start(schedule);-->
<!--                this.convertLastRunToLocalTime();-->
<!--            })-->
<!--            app.success(this.t("schedulesave"), 5000);-->
<!--        },-->
<!--        async stopSchedule() {-->
<!--            await app.load(async () => {-->
<!--                this.values.schedule = await zohoScheduleApi.stop();-->
<!--                this.convertLastRunToLocalTime();-->
<!--                if (!this.values.schedule.type) {-->
<!--                    this.values.schedule.type = "TIME";-->
<!--                }-->
<!--                if (!this.values.schedule.time) {-->
<!--                    this.values.schedule.time = "12:00";-->
<!--                }-->
<!--            })-->
<!--            app.success(this.t("schedulestop"), 5000);-->
<!--        }-->
<!--    }-->
<!--})-->
<!--</script>-->