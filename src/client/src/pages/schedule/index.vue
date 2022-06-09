<script setup>
import {onMounted, ref} from 'vue'
import {useI18n} from 'vue-i18n'
import { getVerifyTokenApi } from '@/api/resources/ZohoToken.js'

const { t } = useI18n()
const status = ref(null)

onMounted(async () => {
  status.value = await getVerifyTokenApi()
})
</script>

<template>
  <div>
    <b>authorized: {{ status }}</b>
    <h1>Zoho index page</h1>
    {{ t('hello') }}
    <div>
      <router-link data-cy="link-about" to="/about">About</router-link>
    </div>
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

export default {
    name: "schedule",
    data() {
        return {
            urls: {
                zohoScheduleApi: "api/zohoschedule"
            },
            time: null,
            isShowTime: false,
            types: [
                { text: this.t("time"), value: "TIME" }
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
        // moment.locale(this.$i18n.locale);
        // this.$store.commit("showLoading");
        try {
            const response = await zohoScheduleApi.get();
            this.values.schedule = response;
            this.convertLastRunToLocalTime();
        } catch (error) {
            console.error(error);
        } finally {
            // this.$store.commit("closeLoading");
        }
    },
    methods: {
        convertLastRunToLocalTime() {
            const lastRun = this.values.schedule.lastRunDate;
            this.values.schedule.lastRunDate = moment(moment.utc(lastRun).toDate()).local().format("LLLL");
        },
        async setSchedule() {
            // this.$store.commit("showLoading");
            
            let time = "";
            const type = this.values.schedule.type;
            time = this.values.schedule.time;

            try {
                const schedule = this.values.schedule;
                schedule.TimeZoneOffSet = (new Date()).getTimezoneOffset().toString();
                schedule.Time = time;
                schedule.Type = type;
                
                const response = await zohoScheduleApi.start(schedule);
                this.values.schedule = response;
                this.convertLastRunToLocalTime();
                // this.$store.commit("notify.success", { content: this.t("schedulesave"), timeout:3000 });

            } catch (error) {
                console.log(error);
            } finally {
                // this.$store.commit("closeLoading");
            }
        },
        async stopSchedule() {
            // this.$store.commit("showLoading");
            try {
                const response = await zohoScheduleApi.stop();
                this.values.schedule = response;
                this.convertLastRunToLocalTime();
                if (!this.values.schedule.type) {
                    this.values.schedule.type = "TIME";
                }
                if (!this.values.schedule.time) {
                    this.values.schedule.time = "12:00";
                }
                // this.$store.commit("notify.success", { content: this.t("schedulestop"), timeout:3000 });
            } catch (error) {
                const resMessage = error.response?.data?.message;
                const errorDetail = JSON.parse(resMessage);
                if (errorDetail) {
                    this.$store.commit("notify.error", { content: errorDetail.message});
                }
            } finally {
                // this.$store.commit("closeLoading");
            }
        }
    }
};
</script>
