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
    
    <v-table fixed-header>
        <thead>
            <tr>
                <th colspan="2">
                    <v-row justify="start">
                        <v-select
                        :items="dateRanges"
                        v-model="selectDateRange"
                        item-text="text"
                        item-value="value"
                        :label="t('completeon')"
                        dark
                        @change="changeWeek"
                    />
                    <v-menu bottom left>
                        <template v-slot:activator="{ on }">
                        <v-btn style="margin-top: 20px;"
                            dark
                            icon
                            v-on="on"
                        >
                            <v-icon>fas fa-info</v-icon>
                        </v-btn>
                        </template>

                        <v-list>
                            <v-list-item
                            >
                            <v-alert
                                dense
                                outlined
                                type="warning"
                                dismissible
                                >
                                <p>This week items include all items of active sprint that have been started on last week and this week and not done yet(Open items)
                                and all items of active sprints that have been done on this week(Closed items).
                                </p>
                                <p>Last week items include all items of active and closed sprint that have been started on lastweek and not done yet(Open items).
                                and all items of active and closed sprints that have been done on this week or last week(Closed items).
                                </p>
                            </v-alert>
                            </v-list-item>
                        </v-list>
                    </v-menu>
                    </v-row>
                </th>
                <th colspan="3">
                    <!-- <v-select
                        ref="assignees"
                        :items="assignees"
                        v-model="values.assignee"
                        v-show="assignees.length > 0"
                        item-text="displayName"
                        item-value="id"
                        :label="t('assignee')"
                        dark
                        @change="search"
                    /> -->
                </th>
                <th :colspan="totalDateColumn">
                    <v-row justify="start">
                        <v-checkbox
                            v-for="(date) in daysOfWeek"
                            v-model="selecteddayOfWeek"
                            :key="`chk-${date.dayOfWeek}`"
                            :label="date.dayOfWeek"
                            :value="date.dayOfWeek"
                            class="ma-0 chkdate"
                            dark
                            hide-details
                        />
                    </v-row>
                </th>
            </tr>
            <tr>
                <th width="20%">{{ t("project") }}</th>
                <th width="25%">{{ t("item") }}</th>
                <th width="5%">{{ t("status") }}</th>
                <th width="5%">{{ t("done") }}</th>
                <th>{{ t("estimatepoints") }}</th>
                <th 
                    v-for="(date, dateIndex) in daysOfWeek"
                    :key="`date-${dateIndex}`"
                    v-show="selecteddayOfWeek.includes(date.dayOfWeek)">
                    {{ date.dayOfWeek }}
                    <br/> {{ date.shortdate }}
                </th>
            </tr>
        </thead>
        <tbody>
            <div v-for="(proj, index) in values.data" :key="`project-${index}`">
                <tr >
                <td :rowspan="proj.tasks.length ? (proj.tasks.length + 1) : 1">
                    <a :href="`${zohoSprintLink}#board/P${proj.projNo}`" target="_blank">{{ proj.name }}</a>
                </td>
            </tr>
            <tr v-for="(task, taskIndex) in proj.tasks" :key="`task-${index}-${taskIndex}`">
                <td :style="`padding-left: ${(task.indent + 1) * 25}px`">
                    <v-icon v-if="task.projItemName === 'Task'" color="blue" small>fas fa-file-alt</v-icon>
                    <v-icon v-else-if="task.projItemName === 'Bug'" color="pink" small>fas fa-bug</v-icon>
                    <v-icon v-else-if="task.projItemName === 'Story'" color="green" small>fas fa-flag</v-icon>
                    <a :href="`${zohoSprintLink}#itemdetails/P${proj.projNo}/I${task.itemNo}`" target="_blank" class="pl-2">
                        {{ task.itemName }}
                    </a>
                </td>
                <td>
                    <v-chip v-show="!['To do', 'In progress', 'Done'].includes(task.statusName)" color="yellow darken-4" text-color="white">Unknown</v-chip>
                    <v-chip v-show="task.statusName === 'To do'" color="pink" text-color="white">To do</v-chip>
                    <v-chip v-show="task.statusName === 'In progress'" color="blue darken-4" text-color="white">In progress</v-chip>
                    <v-chip v-show="task.statusName === 'Done'" color="teal" text-color="white">Done</v-chip>
                </td>
                <td class="text-center">
                    <v-btn v-show="['To do', 'In progress', 'Done'].includes(task.statusName) && task.statusName !== 'Done' && isSelectedLoginUser" color="light-blue darken-4" @click="updateStatus(task)" dark fab x-small>
                        <v-icon>fas fa-check</v-icon>
                    </v-btn>
                </td>
                <td>{{ task.estimatePoint }}</td>
                <td v-for="(logWork, logWorkIndex) in task.logWorks" :key="`logwork-${logWorkIndex}`" v-show="selecteddayOfWeek.includes(logWork.dayOfWeek)">
                    <div v-for="log in logWork.logs" :key="`log-${log.id}`" class="logWork-logs d-flex">
                        <v-text-field
                            type="number"
                            v-model="log.logTime" 
                            :disabled="logWork.isDisabled || !isSelectedLoginUser"
                            :class="`input-${logWork.dayOfWeek}`" 
                            hide-details
                            dense
                            outlined
                            @focus='saveOldLogTime(log.logTime)'
                            @blur='save($event, task, logWork, log)'>
                        </v-text-field>
                        <v-progress-circular indeterminate class="ml-2 d-none"/>
                    </div>
                </td>
            </tr>
            </div>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="5" class="has-background-info">{{ t("total") }}</td>
                <td class="has-background-info"
                    v-for="(date, dateIndex) in daysOfWeek"
                    v-show="selecteddayOfWeek.includes(date.dayOfWeek)"
                    :ref="`total-${date.dayOfWeek}`"
                    :key="`total-${dateIndex}`">
                    {{ date.total }}
                </td>
            </tr>
        </tfoot>
    </v-table>
  </div>
</template>
<script>
import "./index.scss"
import moment from "moment"
import projectApi from '@/api/resources/project'
import logworkApi from '@/api/resources/logwork'
import itemApi from '@/api/resources/item'
import adlsApi from '@/api/resources/adls'

export default {
    data() {
        return {
            dateRanges: [
                {
                    text: this.t("thisweek"),
                    value: "thisweek"
                },
                {
                    text: this.t("lastweek"),
                    value: "lastweek"
                }
            ],
            assignees: [],
            selectDateRange: "thisweek",
            urls: {
                tokenApi: "api/zohotoken",
                itemApi: "api/taskitem",
                logWorkApi: "api/logwork",
                userApi: "api/user"
            },
            values: {
                accessToken: "",
                data: [],
                usersData: [],
                sortTaskItems: [],
                assignee: null,
                userLoginId: null,
                oldLogTime: ""
            },
            daysOfWeek: [],
            startdayOfWeek:  moment().startOf("week"),
            selecteddayOfWeek: [ this.t("mon"), this.t("tue"), this.t("wed"), this.t("thu"), this.t("fri") ],
            dayOfWeek: [],
            estimatedPointVals : [ 0, 1, 2, 3, 4, 6, 8, 10, 12, 16, 20, 24, 28, 32, 40, 48 ],
            zohoSprintLink: "https://sprints.zoho.com/team/archwaybeats"
        };
    },
    computed: {
        totalDateColumn() {
            return this.selecteddayOfWeek.length;
        },
        isSelectedLoginUser() {
            return this.values.assignee === this.values.userLoginId ? true : false;
        }
    },
    async created() {

        await this.loadUsers();

        await this.search();
    },
    methods: {
        async loadUsers() {
            // const resUsers = await axios.get(this.urls.userApi);
            // this.assignees = resUsers.data;
            // this.values.assignee = _.find(this.assignees, { emailId : window.email }).id;
            // this.values.userLoginId = this.values.assignee;
            // const resUser = await axios.get(`${this.urls.userApi}/${window.zuid}`);
            this.values.assignee = "52523000000003787";
            this.values.userLoginId = this.values.assignee;
        },
        async search() {
            this.getWeekDateData();

            this.values.data = [];
            this.values.logWorkData = [];
            this.values.sortTaskItems = [];

            // // this.$store.commit("showLoading");

            try 
            {
                const sprintTypeIds = this.selectDateRange === "thisweek" ? [ "2" ] :[ "2", "3" ],
                    assignees = !this.isSelectedLoginUser ? [this.values.assignee] : null,
                    openTaskCondition = {
                        sprintTypeIds,
                        statusId : 0,
                        startDateFrom: this.selectDateRange === "thisweek" ? null: new Date(moment(this.startdayOfWeek).add(0, "days")),
                        startDateTo: this.selectDateRange === "thisweek" ? null : new Date(moment(this.startdayOfWeek).add(6, "days")),
                        completedOn : [],
                        assignees
                    },
                    closedTaskCondition = { sprintTypeIds, statusId : 1, completedOn : this.selectDateRange === "thisweek" ? [ "thisweek" ] : [ "thisweek", "lastweek" ], assignees};

                const [ resOpenTaskItems, resClosedTaskItems ]= await Promise.all([ 
                    this.itemApi.find(openTaskCondition), // sprinttype = 2 : Active Sprint, status = 0 : open
                    this.itemApi.find(closedTaskCondition) // sprinttype = 2 : Active Sprint, status = 1 : closed
                ]);

                const allTaskItems = resOpenTaskItems.concat(resClosedTaskItems);

                const logworkSearchCondition = {
                    StartDate: new Date(moment(this.startdayOfWeek).add(1, "days")),
                    EndDate: new Date(moment(this.startdayOfWeek).add(7, "days")),
                    SprintTypeId: 2,
                    ownerIds: this.isSelectedLoginUser ? null : [ this.values.assignee ]
                };

                const reslogWork = await this.logworkApi.searchByGlobalView(logworkSearchCondition);
                this.values.logWorkData = reslogWork;

                if (this.selectDateRange !== "thisweek")
                {
                    const extraTaskData = _.filter(this.values.logWorkData, (logWorkitem) => { return !_.find(allTaskItems, (task) => { return task.id === logWorkitem.itemId && logWorkitem.logTime !== 0; }); });
                    extraTaskData.forEach((taskitem) => {
                        allTaskItems.push({
                            id: taskitem.itemId,
                            itemNo: taskitem.itemNo,
                            itemName: taskitem.itemName,
                            projName: taskitem.projName,
                            projNo: taskitem.projNo
                        });
                    });
                }

                const sortTaskItemsbyId = _.sortBy(allTaskItems, [ "taskItemId" ]);
                let subItemids = [];
                _.forEach(allTaskItems, (item) => {
                    if (item.subItemIds && item.subItemIds.length > 0) {
                        subItemids = subItemids.concat(item.subItemIds);
                    }
                });

                //sort items by subitems
                sortTaskItemsbyId.forEach(item => {
                    item.estimatePoint = this.estimatedPointVals[item.points];
                    const exist = _.find(this.values.sortTaskItems, { id: item.id });
                    const isSubItems = _.includes(subItemids, item.id);
                    if (!exist && !isSubItems) {
                        item.indent = 0;
                        this.values.sortTaskItems.push(item);
                        if (item.isParent) {
                            this.getSubItemsOfItem(item, item.subItemIds, allTaskItems);
                        }
                    }
                });

                const projectsData = _(this.values.sortTaskItems)
                    .groupBy("projName")
                    .map((items, projName) => {
                        let defaultItem = _.find(items, (item) => { return item.id && item.projNo; });
                        if (!defaultItem) {
                            defaultItem = items[0];
                        }

                        return {
                            id: defaultItem.id,
                            name: projName,
                            tasks: items,
                            projNo: defaultItem.projNo
                        };
                    })
                    .value();

                projectsData.forEach(proj => {
                    proj.tasks.forEach(task => {
                        task.logWorks = [];
                        this.daysOfWeek.forEach(date => {
                            let logWorkByDate = _.filter(this.values.logWorkData, (log)=>{
                                return log.itemId === task.id && _.startsWith(log.logDate, date.date);
                            });
                            
                            if (logWorkByDate === null || logWorkByDate.length === 0)
                            {
                                logWorkByDate = [
                                    {
                                        Owner: this.values.assignee,
                                        itemName: task.itemName,
                                        itemNo: task.itemNo,
                                        logDate: date.date,
                                        projItemTypeId: task.projItemTypeId,
                                        logTime: null
                                    }
                                ];
                            }
                            else
                            {
                                _.forEach(logWorkByDate, (item) => {
                                    date.total += item.logTime;
                                });
                            }

                            task.logWorks.push({ dayOfWeek: date.dayOfWeek, date: date.date, logs: logWorkByDate, isDisabled : date.isDisabled});
                        });
                    });
                });
                debugger
                this.values.data = _.orderBy(projectsData, ["name"], ["asc"]);
                this.$forceUpdate();
            } catch (error) {
                console.log(error);
            } finally {
                // // this.$store.commit("closeLoading");
            }
        },
        getSubItemsOfItem(rootItem, subItemIds, allTaskItems) {
            subItemIds.forEach(itemid => {
                const exist = _.find(this.values.sortTaskItems, { id: itemid.id });
                if (!exist) {
                    const task = _.find(allTaskItems, { id : itemid });
                    if (task && (task.depth - 1) === rootItem.depth) {
                        task.indent = rootItem.indent + 1;
                        this.values.sortTaskItems.push(task);
                        if (task.isParent) {
                            this.getSubItemsOfItem(task, task.subItemIds, allTaskItems);
                        }
                    }
                }
                
            });
        },
        getWeekDateData() {
            this.daysOfWeek = [
                { dayOfWeek: this.t("sun"), shortdate: this.startdayOfWeek.format("MM/DD"), date: this.startdayOfWeek.format("MM/DD/YYYY"), total: 0, isDisabled: this.startdayOfWeek.isAfter(moment()) },
                { dayOfWeek: this.t("mon"), shortdate: moment(this.startdayOfWeek).add(1, "days").format("MM/DD"), date: moment(this.startdayOfWeek).add(1, "days").format("MM/DD/YYYY"), total: 0, isDisabled: moment(this.startdayOfWeek).add(1, "days").isAfter(moment()) },
                { dayOfWeek: this.t("tue"), shortdate: moment(this.startdayOfWeek).add(2, "days").format("MM/DD"), date: moment(this.startdayOfWeek).add(2, "days").format("MM/DD/YYYY"), total: 0, isDisabled: moment(this.startdayOfWeek).add(2, "days").isAfter(moment()) },
                { dayOfWeek: this.t("wed"), shortdate: moment(this.startdayOfWeek).add(3, "days").format("MM/DD"), date: moment(this.startdayOfWeek).add(3, "days").format("MM/DD/YYYY"), total: 0, isDisabled: moment(this.startdayOfWeek).add(3, "days").isAfter(moment()) },
                { dayOfWeek: this.t("thu"), shortdate: moment(this.startdayOfWeek).add(4, "days").format("MM/DD"), date: moment(this.startdayOfWeek).add(4, "days").format("MM/DD/YYYY"), total: 0, isDisabled: moment(this.startdayOfWeek).add(4, "days").isAfter(moment()) },
                { dayOfWeek: this.t("fri"), shortdate: moment(this.startdayOfWeek).add(5, "days").format("MM/DD"), date: moment(this.startdayOfWeek).add(5, "days").format("MM/DD/YYYY"), total: 0, isDisabled: moment(this.startdayOfWeek).add(5, "days").isAfter(moment()) },
                { dayOfWeek: this.t("sat"), shortdate: moment(this.startdayOfWeek).add(6, "days").format("MM/DD"), date: moment(this.startdayOfWeek).add(6, "days").format("MM/DD/YYYY"), total: 0, isDisabled: moment(this.startdayOfWeek).add(6, "days").isAfter(moment()) }
            ];
        },
        async changeWeek() {
            this.startdayOfWeek = this.selectDateRange === "thisweek" ? moment().startOf("week") : moment().startOf("week").add(-7, "days");

            this.getWeekDateData();

            await this.search();
        },
        saveOldLogTime(logTime) {
            if(logTime === null || logTime === ""){
                return;
            }
            this.values.oldLogTime = logTime;
        },
        async save($event, task, logWork, log) {
            if (!this.isSelectedLoginUser) {
                return;
            }

            const target = $event.target,
                vInput = target.closest(".v-input"),
                processEle = vInput.nextElementSibling;

            if(log.logTime){
                if (log.logTime.toString() !== this.values.oldLogTime.toString()) {
                    vInput.classList.toggle("v-input--is-disabled");
                    processEle.classList.toggle("d-none");
                    target.toggleAttribute("disabled");
                    log.isloading = true;

                    const duration = parseFloat(log.logTime),
                        decimal = duration % 1;

                    const parameter = {
                        logTimeId: log.id,
                        projid : task.projId,
                        sprintid : task.sprintId,
                        itemid: task.id,
                        duration: decimal === 0 ? `${duration}` : `${Math.floor(duration)}:${decimal * 60}`,
                        users: this.values.assignee,
                        date: moment(logWork.date).format("YYYY-MM-DDTHH:mm:ssZ"),
                        isbillable: 1
                    };

                    try {
                        const axiosCallBack = !log.id ? axios.post : axios.put;
                        const url = !log.id ? `${this.urls.logWorkApi}/create` : this.urls.logWorkApi;

                        const response = await axiosCallBack(url, parameter, { headers: { AccessToken: this.values.accessToken } });

                        if (response.data !== null && response.data !== undefined)
                        {
                            if (!log.id)
                            {
                                log.id = response.data.id;
                            }

                            // this.$store.commit("notify.success", { content: this.t("savesuccess"), timeout:3000 });

                            let total = 0;
                            const inputs = this.$el.querySelectorAll(`.v-input.input-${logWork.dayOfWeek} input`);
                            _.forEach(inputs, (input) => {
                                if (input.value)
                                {
                                    total += parseFloat(input.value);
                                }
                            });

                            this.$refs[`total-${logWork.dayOfWeek}`][0].innerHTML = total; 
                        }

                        vInput.classList.toggle("v-input--is-disabled");
                        vInput.classList.remove("error");
                        target.toggleAttribute("disabled");
                    }
                    catch (error) {
                        const resMessage = error.response?.data?.message;
                        const errorDetail = JSON.parse(resMessage);
                        if (errorDetail && errorDetail.message === "Can add loghours only for active, upcoming and backlog sprint") {
                            vInput.classList.add("error");
                        }
                    } finally {
                        processEle.classList.toggle("d-none");
                    }
                }
            }
        },
        async updateStatus(task) {
            const currentStatus = task.statusName;
            const taskItemStatusParameter = {
                taskItemId: task.id,
                projId: task.projId,
                sprintId: task.sprintId,
                statusId: "52523000000002619"
            };

            await axios.post(`${this.urls.itemApi}/update-status`, taskItemStatusParameter, { headers: { AccessToken: accessToken } });

            // this.$store.commit("notify.success", { content: this.t("savesuccess"), timeout:3000 });

            task.statusName = "Done";
            if (task.isParent)
            {
                _.forEach(task.subItemIds, item => {
                    const subItem = _.find(this.values.data[0].tasks, { id: item});
                    if (subItem.statusName === currentStatus)
                    {
                        subItem.statusName = "Done";
                    }
                });
            }
        },
    }
};
</script>

<route lang="yaml">
meta:
  layout: default
</route>
