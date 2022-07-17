<template>
  <div class="time-sheet" ref="refTimeSheet">
    <v-toolbar class="px-0" style="background-color: #f6f6f6; color: #009688">
      <div class="d-flex align-center justify-space-around">
        <v-select
          v-model="selectDateRange"
          :items="dateRanges"
          :label="t('completeon')"
          @update:modelValue="refresh"
          item-title="text"
          item-value="value"
          class="mt-10"
        />
        <v-tooltip location="end">
          <template v-slot:activator="{ props }">
            <v-btn
              v-bind="props"
              icon="mdi-information-outline"
              variant="text"
              size="20px"
              class="ma-2"
            />
          </template>
          <span color="black">
            <p>{{ t("thisweekinformation") }}</p>
            <p>{{ t("lastweekinformation") }}</p>
          </span>
        </v-tooltip>
      </div>
      <div>
        <v-select
          v-model="assignee"
          :items="assignees"
          :label="t('owner')"
          @update:modelValue="refresh"
          item-title="displayName"
          item-value="userId"
          class="mt-10"
        />
      </div>
      <v-spacer></v-spacer>
      <div class="d-flex justify-space-around mr-1">
        <v-tooltip location="top">
          <template v-slot:activator="{ props }">
            <v-btn class="ma-2 text-capitalize" v-bind="props" @click="refresh">
              <v-icon start icon="mdi-sync" />
              {{ t("button.refresh") }}
            </v-btn>
          </template>
          <span>{{ t("reloadData") }}</span>
        </v-tooltip>
        <v-menu location="start" :close-on-content-click="false">
          <template v-slot:activator="{ props }">
            <v-btn v-bind="props" icon="mdi-dots-vertical" />
          </template>
          <v-card min-width="130">
            <v-list nav dense>
              <h5 class="text-blue">{{ t("dayOfWeeks") }}</h5>
              <v-list-item
                v-for="date in daysOfWeek"
                :key="`chk${date.dayOfWeek}`"
              >
                <v-checkbox
                  v-model="selecteddayOfWeek"
                  :label="date.dayOfWeek"
                  :value="date.dayOfWeek"
                  class="my-n4"
                  dark
                  hide-details
                />
              </v-list-item>
            </v-list>
          </v-card>
        </v-menu>
        <v-btn
          icon="mdi-plus"
          @click="toggleItemDrawer"
        />
      </div>
    </v-toolbar>
    <v-table fixed-header fixed-footer height="calc(100vh - 170px)">
      <thead>
        <tr class="elevation-5">
          <th width="20%">{{ t("project") }}</th>
          <th width="25%">{{ t("item") }}</th>
          <th width="5%">{{ t("status") }}</th>
          <th width="5%">{{ t("done") }}</th>
          <th>{{ t("estimatepoints") }}</th>
          <th
            v-for="(date, dateIndex) in daysOfWeek"
            :key="`date-${dateIndex}`"
            v-show="selecteddayOfWeek.includes(date.dayOfWeek)"
          >
            {{ date.dayOfWeek }}
            <br />
            {{ date.shortDate }}
          </th>
        </tr>
      </thead>
      <tbody>
        <template
          v-for="(proj, index) in values.data"
          :key="`project-${index}`"
        >
          <tr>
            <td :rowspan="proj.tasks.length ? proj.tasks.length + 1 : 1">
              <a
                :href="`${zohoSprintLink}#board/P${proj.projNo}`"
                target="_blank"
                >{{ proj.name }}</a
              >
            </td>
          </tr>
          <tr
            v-for="(task, taskIndex) in proj.tasks"
            :key="`task-${index}-${taskIndex}`"
          >
            <td :style="`padding-left: ${(task.indent + 1) * 25}px`">
              <v-icon
                v-if="task.projItemTypeName === 'Task'"
                color="blue"
                small
                icon="mdi-file"
              ></v-icon>
              <v-icon
                v-else-if="task.projItemTypeName === 'Bug'"
                color="pink"
                small
                icon="mdi-bug"
              ></v-icon>
              <v-icon
                v-else-if="task.projItemTypeName === 'Story'"
                color="green"
                small
                icon="mdi-flag"
              ></v-icon>
              <a
                :href="`${zohoSprintLink}#itemdetails/P${proj.projNo}/I${task.itemNo}`"
                target="_blank"
                class="pl-2"
              >
                {{ task.itemName }}
              </a>
            </td>
            <td>
              <v-chip
                v-show="
                  !['To do', 'In progress', 'Done'].includes(task.statusName)
                "
                color="yellow darken-4"
                text-color="white"
                >Unknown</v-chip
              >
              <v-chip
                v-show="task.statusName === 'To do'"
                color="pink"
                text-color="white"
                >To do</v-chip
              >
              <v-chip
                v-show="task.statusName === 'In progress'"
                color="blue darken-4"
                text-color="white"
                >In progress</v-chip
              >
              <v-chip
                v-show="task.statusName === 'Done'"
                color="teal"
                text-color="white"
                >Done</v-chip
              >
            </td>
            <td class="text-center">
              <v-icon
                v-show="
                  ['To do', 'In progress', 'Done'].includes(task.statusName) &&
                  task.statusName !== 'Done'
                "
                color="light-blue darken-4"
                @click="updateStatus(task)"
                dark
                fab
                x-small
                icon="mdi-check-outline"
                start
              >
              </v-icon>
            </td>
            <td>{{ task.estimatePoint }}</td>
            <td
              v-for="(logWork, logWorkIndex) in task.logWorks"
              :key="`logwork-${logWorkIndex}`"
              v-show="selecteddayOfWeek.includes(logWork.dayOfWeek)"
            >
              <div
                v-for="log in logWork.logs"
                :key="`log-${log.id}`"
                class="logWork-logs d-flex"
              >
                <v-text-field
                    type="number"
                    v-model="log.logTime"
                    :disabled="logWork.isDisabled || assignee != app.zsUserId"
                    :class="`input-${logWork.dayOfWeek}`"
                    hide-details
                    dense
                    outlined
                    @focus='saveOldLogTime(log.logTime)'
                    @blur='save($event, task, logWork, log)'>
                </v-text-field>
                <v-progress-circular indeterminate class="ml-2 d-none" />
              </div>
            </td>
          </tr>
        </template>
      </tbody>
      <tfoot>
        <tr class="elevation-5">
          <td colspan="5" class="has-background-info">{{ t("total") }}</td>
          <td
            class="has-background-info"
            v-for="(date, dateIndex) in daysOfWeek"
            v-show="selecteddayOfWeek.includes(date.dayOfWeek)"
            :ref="daysOfWeek"
            :key="`total${dateIndex}`"
          >
            {{ date.total }}
          </td>
        </tr>
      </tfoot>
    </v-table>

    <ItemDrawer v-model="itemDrawerModel" :assignees="assignees" @afterCreate="afterCreateItem"/>
  </div>
</template>

<script setup>
import "./index.scss";
import moment from "moment";
import { useI18n } from "vue-i18n";
import { ref, reactive, computed, onBeforeMount } from "vue";
import appStore from "@/store/app";
import logworkApi from "@/api/resources/logwork";
import itemApi from "@/api/resources/item";
import { getAllUser} from '@/api/resources/user'

import ItemDrawer from '@/components/organisms/ItemDrawer.vue'

const { t } = useI18n()
const app = appStore()

const dateRanges = reactive([
  { text: t("thisweek"), value: "thisweek" },
  { text: t("lastweek"), value: "lastweek" },
]);
const selectDateRange = ref("thisweek");
const values = reactive({
  accessToken: "",
  data: [],
  usersData: [],
  sortTaskItems: [],
  oldLogTime: "",
});
const daysOfWeek = ref([]);
const selecteddayOfWeek = ref([
  t("mon"),
  t("tue"),
  t("wed"),
  t("thu"),
  t("fri"),
]);
const estimatedPointVals = reactive([
  0, 1, 2, 3, 4, 6, 8, 10, 12, 16, 20, 24, 28, 32, 40, 48
])

let assignees = ref([])
let assignee = ref(null)

const refTimeSheet = ref(null)
const startdayOfWeek = ref(moment().startOf('week'))
const zohoSprintLink = 'https://sprints.zoho.com/team/archwaybeats'
const itemDrawerModel = ref(false)

// a computed selecteddayOfWeek
const totalDateColumn = computed(() => {
  return selecteddayOfWeek.value.length;
});

function toggleItemDrawer() {
  itemDrawerModel.value = !itemDrawerModel.value
}

// handle a methods
function getWeekDateData() {
  daysOfWeek.value = [];
  for (let index = 0; index < 7; index++) {
    var date = moment(startdayOfWeek.value).add(index, "days");

    daysOfWeek.value.push({
      index,
      dayOfWeek: date._d.toLocaleString(undefined, { weekday: "short" }),
      date,
      shortDate: date.format("MM/DD"),
      longDate: date.format("MM/DD/YYYY"),
      total: 0,
    });
  }
}

function arrangeItem(allTaskItems, item, isIndent) {
  const existItem = _.find(values.sortTaskItems, { id: item.id });
  if (!existItem) {
    item.estimatePoint = estimatedPointVals[item.points];
    item.indent = isIndent ? item.depth : 0;
    values.sortTaskItems.push(item);
    if (item.isParent) {
      var subItems = allTaskItems.filter(i => i.parentItemId == item.id);
      subItems.forEach(i => {
        i.indent = i.depth;
        arrangeItem(allTaskItems, i, true);

        const existSubItem = _.find(values.sortTaskItems, { id: i.id });
        if (!existSubItem) values.sortTaskItems.push(i);
      })
    }
  }
}

async function loadUsers() {
  const response = await getAllUser();
    
  if (response !== null && response !== undefined)
  {
    assignees.value = response;
    assignee.value = app.zsUserId != null ? app.zsUserId : assignees[0];
  }
}

async function search() {
  values.data = [];
  values.sortTaskItems = [];

  try {
    const sprintTypeIds =
        selectDateRange.value === "thisweek" ? ["2"] : ["2", "3"],
      openTaskCondition = {
        sprintTypeIds,
        statusId: 0,
        startDateFrom: selectDateRange.value === "thisweek" ? null : new Date(daysOfWeek.value[0].date._d),
        startDateTo: selectDateRange.value === "thisweek" ? null : new Date(daysOfWeek.value[6].date._d),
        assignees: [assignee.value]
      },
      closedTaskCondition = {
        sprintTypeIds,
        statusId: 1,
        completedOn:
          selectDateRange.value === "thisweek"
            ? ["thisweek"]
            : ["thisweek", "lastweek"],
        assignees: [assignee.value]
      },
      logworkSearchCondition = {
        StartDate: new Date(daysOfWeek.value[0].date._d),
        EndDate: new Date(daysOfWeek.value[6].date._d),
        ownerIds: [assignee.value],
      };

    await app.load(async () => {
      const [resOpenTaskItems, resClosedTaskItems, resLogWorks ] = await Promise.all([
        itemApi.find(openTaskCondition), // sprinttype = 2 : Active Sprint, status = 0 : open
        itemApi.find(closedTaskCondition), // sprinttype = 2 : Active Sprint, status = 1 : closed
        logworkApi.find(logworkSearchCondition)
      ]);

      const allTaskItems = resOpenTaskItems.concat(resClosedTaskItems);
      const logWorkData = resLogWorks;

      if (selectDateRange.value !== "thisweek") {
        const extraTaskData = _.filter(logWorkData, (logWorkitem) => {
          var existItem = _.find(allTaskItems, (task) => {
            return task.id === logWorkitem.itemId;
          });

          return !existItem;
        });

        extraTaskData.forEach((taskitem) => {
          allTaskItems.push({
            id: taskitem.itemId,
            itemNo: taskitem.itemNo,
            itemName: taskitem.itemName,
            projName: taskitem.projName,
            projNo: taskitem.projNo,
          });
        });
      }

      const sortTaskItemsbyId = _.orderBy(allTaskItems, ["isParent", "id"], ["desc", "asc"]);
      let subItemids = [];
      _.forEach(allTaskItems, (item) => {
        if (item.subItemIds && item.subItemIds.length > 0) {
          subItemids = subItemids.concat(item.subItemIds);
        }
      });

      //sort items by subitems
      sortTaskItemsbyId.forEach((item) => {
        arrangeItem(allTaskItems, item);
      });

      const timesheetData = _(values.sortTaskItems)
        .groupBy("projName")
        .map((items, projName) => {
          let defaultItem = _.find(items, (item) => {
            return item.id && item.projNo;
          });
          if (!defaultItem) {
            defaultItem = items[0];
          }

          return {
            projId: defaultItem.projId,
            name: projName,
            tasks: items,
            projNo: defaultItem.projNo,
          };
        })
        .value();

      timesheetData.forEach((proj) => {
        proj.tasks.forEach((task) => {
          task.logWorks = [];
          daysOfWeek.value.forEach((date) => {
            let logWorkByDate = _.filter(logWorkData, (log) => {
              return (
                log.itemId === task.id && _.startsWith(log.logDate, date.longDate)
              );
            });

            if (logWorkByDate === null || logWorkByDate.length === 0) {
              logWorkByDate = [
                {
                  itemName: task.itemName,
                  itemNo: task.itemNo,
                  logDate: date.longDate,
                  projItemTypeId: task.projItemTypeId,
                  logTime: null,
                },
              ];
            } else {
              _.forEach(logWorkByDate, (item) => {
                date.total += item.logTime;
              });
            }

            task.logWorks.push({
              dayOfWeekIndex: date.index,
              dayOfWeek: date.dayOfWeek,
              date: date.longDate,
              logs: logWorkByDate,
              isDisabled: moment(date.longDate).isAfter(moment()),
            });
          });
        });
      });

      values.data = _.orderBy(timesheetData, ["name"], ["asc"]);
    });
  } catch (error) {
    console.log(error);
  }
}

async function refresh() {
  startdayOfWeek.value =
    selectDateRange.value === "thisweek"
      ? moment().startOf("week")
      : moment().startOf("week").add(-7, "days");

  getWeekDateData();
  await search();
}

async function save($event, task, logWork, log) {
  const target = $event.target,
    vInput = target.closest(".v-input"),
    processEle = vInput.nextElementSibling;

  if (log.logTime !== null && log.logTime !== undefined) {
    if (log.logTime.toString() !== values.oldLogTime.toString()) {
      vInput.classList.toggle("v-input--is-disabled");
      processEle.classList.toggle("d-none");
      target.toggleAttribute("disabled");
      log.isloading = true;

      const duration = parseFloat(log.logTime),
        decimal = duration % 1;

      const parameter = {
        logTimeId: log.logTimeId,
        projid: task.projId,
        sprintid: task.sprintId,
        itemid: task.id,
        duration:
          decimal === 0
            ? `${duration}`
            : `${Math.floor(duration)}:${decimal * 60}`,
        users: app.zsUserId,
        date: moment(logWork.date).format("YYYY-MM-DDTHH:mm:ssZ"),
        isbillable: 1,
        actionField: null,
      };
      try {
        const isUpdate = log.logTimeId ? true : false;
        const response = !log.logTimeId
          ? await logworkApi.create(parameter)
          : await logworkApi.update(parameter);

        if ((response !== null && response !== undefined) || isUpdate) {
          if (!log.logTimeId) {
            log.logTimeId = response.logTimeId;
          }

          app.success(t("savesuccess"), 5000);

          let total = 0;
          const inputs = refTimeSheet.value.querySelectorAll(
            `.v-input.input-${logWork.dayOfWeek} input`
          );
          _.forEach(inputs, (input) => {
            if (input.value) {
              total += parseFloat(input.value);
            }
          });

          daysOfWeek.value[logWork.dayOfWeekIndex].total = total;
        }

        vInput.classList.toggle("v-input--is-disabled");
        vInput.classList.remove("error");
        target.toggleAttribute("disabled");
      } catch (error) {
        const resMessage = error.response?.data?.message;
        const errorDetail = JSON.parse(resMessage);
        if (
          errorDetail &&
          errorDetail.message ===
            "Can add loghours only for active, upcoming and backlog sprint"
        ) {
          vInput.classList.add("error");
        }
      } finally {
        processEle.classList.toggle("d-none");
      }
    }
  }
}

async function updateStatus(task) {
  const currentStatus = task.statusName;
  const taskItemStatusParameter = {
    taskItemId: task.id,
    projId: task.projId,
    sprintId: task.sprintId,
    statusId: "52523000000002619",
  };
  await app.load(async () => {
    await itemApi.updateStatus(taskItemStatusParameter);
  });

  app.success(t("savesuccess"), 5000);

  task.statusName = "Done";
  if (task.isParent) {
    _.forEach(task.subItemIds, (item) => {
      const subItem = _.find(values.data[0].tasks, { id: item });
      if (subItem.statusName === currentStatus) {
        subItem.statusName = "Done";
      }
    });
  }
}

function saveOldLogTime(logTime) {
  if (logTime === null || logTime === "") {
    return;
  }
  values.oldLogTime = logTime;
}

function afterCreateItem(item) {
  let selectedProject = values.data.find(p => p.projId === item.projId);

  if (!selectedProject) {
    const newProject = {
      projId: item.projId,
      name: item.projName,
      tasks: [],
      projNo: item.projNo,
    };

    selectedProject = newProject;

    values.data.push(selectedProject);
  }

  var newItem = {
    id: item.projId,
    itemNo: item.itemNo,
    itemName: item.itemName,
    projName: item.projName,
    projNo: item.projNo,
    projItemTypeName: item.projItemTypeName,
    statusName: "To do",
    indent: 0,
    logWorks: []
  }

  daysOfWeek.value.forEach((date) => {
    newItem.logWorks.push({
      dayOfWeekIndex: date.index,
      dayOfWeek: date.dayOfWeek,
      date: date.longDate,
      logs: [{
        itemName: item.itemName,
        itemNo: item.itemNo,
        logDate: date.longDate,
        projItemTypeId: item.projItemTypeId,
        logTime: null,
      }],
      isDisabled: moment(date.longDate).isAfter(moment()),
    })
  })

  selectedProject.tasks.push(newItem)
}

// bind data
onBeforeMount(async () => {
  await loadUsers()
  getWeekDateData()
  await search()
})
</script>

<style scoped>
.view {
  display: flex;
  flex-direction: column;
  height: 100%;
  padding: 20px;
}

.card-top {
  padding: 20px;
}

.table-container {
  display: flex;
  margin-top: 20px;
  flex-grow: 1;
  overflow: hidden;
}

.flex-table {
  display: flex;
  flex-grow: 1;
  width: 100%;
}

.flex-table > div {
  width: 100%;
}
</style>