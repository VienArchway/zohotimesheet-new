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

<route lang="yaml">
meta:
  layout: default
</route>

<script>
import "./index.scss"
import moment from "moment"
import zohoScheduleApi from '@/api/resources/zohoschedule'
import appStore from '@/store/app'
import { useI18n } from 'vue-i18n'
import { defineComponent } from 'vue'

const app = appStore()

export default defineComponent({
    name: "schedule",
    setup() {
      const { t } = useI18n()
      return { t }
    },
    data() {
        return {
            urls: {
                zohoScheduleApi: "api/zohoschedule"
            },
            time: null,
            isShowTime: false,
            types: [
                { text: "time", value: "TIME" }
            ],
            values: {
                schedule: {
                    type: "TIME",
                    time: "12:00",
                    lastRunDate: null,
                    status: null
                }
            }
        };
    },
    async created() {
        await app.load(async () => {
            const response = await zohoScheduleApi.get();
            this.values.schedule = response;
            this.convertLastRunToLocalTime();
        })
    },
    methods: {
        convertLastRunToLocalTime() {
            const lastRun = this.values.schedule.lastRunDate;
            this.values.schedule.lastRunDate = moment(moment.utc(lastRun).toDate()).local().format("LLLL");
        },
        async setSchedule() {
            let time = "";
            const type = this.values.schedule.type;
            time = this.values.schedule.time;

            await app.load(async () => {
                const schedule = this.values.schedule;
                schedule.TimeZoneOffSet = (new Date()).getTimezoneOffset().toString();
                schedule.Time = time;
                schedule.Type = type;
                
                const response = await zohoScheduleApi.start(schedule);
                this.values.schedule = response;
                this.convertLastRunToLocalTime();
            })
            app.success(this.t("schedulesave"), 5000);
        },
        async stopSchedule() {
            await app.load(async () => {
                const response = await zohoScheduleApi.stop();
                this.values.schedule = response;
                this.convertLastRunToLocalTime();
                if (!this.values.schedule.type) {
                    this.values.schedule.type = "TIME";
                }
                if (!this.values.schedule.time) {
                    this.values.schedule.time = "12:00";
                }
            })
            app.success(this.t("schedulestop"), 5000);
        }
    }
})
</script>
