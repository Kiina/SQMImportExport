﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQMImportExport.ArmA2;
using SQMImportExport.Common;

namespace SQMImportExport.Export.ArmA2
{
    internal class SqmElementExportVisitor : ISqmElementVisitor
    {
        private readonly SqmPropertyVisitor _propertyVisitor = new SqmPropertyVisitor();

        public string Visit(string elementName, SqmContents sqmContents)
        {
            var fileString = new StringBuilder();

            fileString.Append(_propertyVisitor.Visit("version", sqmContents.Version));

            fileString.Append(Visit("Mission", sqmContents.Mission));
            fileString.Append(Visit("Intro", sqmContents.Intro));
            fileString.Append(Visit("OutroWin", sqmContents.OutroWin));
            fileString.Append(Visit("OutroLoose", sqmContents.OutroLose));

            return fileString.ToString();
        }

        public string Visit(string elementName, MissionState mission)
        {
            if (mission == null)
            {
                return "";
            }

            var missionString = new StringBuilder();

            missionString.Append("class ");
            missionString.Append(elementName);
            missionString.Append("\n");
            missionString.Append("{\n");
            missionString.Append(_propertyVisitor.Visit("addOns", mission.AddOns));
            missionString.Append(_propertyVisitor.Visit("addOnsAuto", mission.AddOnsAuto));
            missionString.Append(_propertyVisitor.Visit("randomSeed", mission.RandomSeed));
            missionString.Append(Visit("Intel", mission.Intel));
            missionString.Append(Visit("Groups", mission.Groups));
            missionString.Append(Visit("Vehicles", mission.Vehicles));
            missionString.Append(Visit("Markers", mission.Markers));
            missionString.Append(Visit("Sensors", mission.Sensors));
            missionString.Append("};\n");

            return missionString.ToString();
        }

        public string Visit(string elementName, Intel intel)
        {
            if (intel == null)
            {
                return "";
            }

            var intelString = new StringBuilder();

            intelString.Append("class " + elementName + "\n");
            intelString.Append("{\n");
            intelString.Append(_propertyVisitor.Visit("briefingName", intel.BriefingName));
            intelString.Append(_propertyVisitor.Visit("briefingDescription", intel.BriefingDescription));
            intelString.Append(_propertyVisitor.Visit("resistanceWest", intel.ResistanceWest));
            intelString.Append(_propertyVisitor.Visit("resistanceEast", intel.ResistanceEast));
            intelString.Append(_propertyVisitor.Visit("startWeather", intel.StartWeather));
            intelString.Append(_propertyVisitor.Visit("startFog", intel.StartFog));
            intelString.Append(_propertyVisitor.Visit("forecastWeather", intel.ForecastWeather));
            intelString.Append(_propertyVisitor.Visit("forecastFog", intel.ForecastFog));
            intelString.Append(_propertyVisitor.Visit("year", intel.Year));
            intelString.Append(_propertyVisitor.Visit("month", intel.Month));
            intelString.Append(_propertyVisitor.Visit("day", intel.Day));
            intelString.Append(_propertyVisitor.Visit("hour", intel.Hour));
            intelString.Append(_propertyVisitor.Visit("minute", intel.Minute));
            intelString.Append("};\n");

            return intelString.ToString();
        }

        private string Visit(
            string elementName,
            List<VehicleBase> items,
            Func<string, VehicleBase, string> getItemString)
        {
            if (items == null || items.Count == 0)
            {
                return "";
            }

            var itemsString = new StringBuilder();

            itemsString.Append("class " + elementName + "\n");
            itemsString.Append("{\n");
            itemsString.Append(_propertyVisitor.Visit("items", items.Count));

            foreach (var subItem in items)
            {
                itemsString.Append(getItemString("Item", subItem));
            }

            itemsString.Append("};\n");

            return itemsString.ToString();
        }

        public string Visit(string elementName, IEnumerable<Vehicle> vehicles)
        {
            return Visit(elementName, vehicles.Cast<VehicleBase>().ToList(),
                (itemName, item) => Visit(itemName, (Vehicle) item));
        }

        private string Visit(string elementName, List<Waypoint> waypoints)
        {
            return Visit(elementName, waypoints.Cast<VehicleBase>().ToList(),
                (itemName, item) => Visit(itemName, (Waypoint) item));
        }

        public string Visit(string elementName, List<Marker> markers)
        {
            return Visit(elementName, markers.Cast<VehicleBase>().ToList(),
                (itemName, item) => Visit(itemName, (Marker) item));
        }

        public string Visit(string elementName, List<Sensor> sensors)
        {
            return Visit(elementName, sensors.Cast<VehicleBase>().ToList(),
                (itemName, item) => Visit(itemName, (Sensor) item));
        }

        public string Visit(string elementName, Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return "";
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("class " + elementName + vehicle.Number + "\n");
            stringBuilder.Append("{\n");
            stringBuilder.Append(_propertyVisitor.Visit("presence", vehicle.Presence));
            stringBuilder.Append(_propertyVisitor.Visit("presenceCondition", vehicle.PresenceCondition));
            stringBuilder.Append(_propertyVisitor.Visit("position", vehicle.Position));
            stringBuilder.Append(_propertyVisitor.Visit("placement", vehicle.Placement));
            stringBuilder.Append(_propertyVisitor.Visit("azimut", vehicle.Azimut));
            stringBuilder.Append(_propertyVisitor.Visit("special", vehicle.Special));
            stringBuilder.Append(_propertyVisitor.Visit("age", vehicle.Age));
            stringBuilder.Append(_propertyVisitor.Visit("id", vehicle.Id));
            stringBuilder.Append(_propertyVisitor.Visit("side", vehicle.Side));
            stringBuilder.Append(_propertyVisitor.Visit("vehicle", vehicle.VehicleName));
            stringBuilder.Append(_propertyVisitor.Visit("player", vehicle.Player));
            stringBuilder.Append(_propertyVisitor.Visit("leader", vehicle.Leader));
            stringBuilder.Append(_propertyVisitor.Visit("lock", vehicle.Lock));
            stringBuilder.Append(_propertyVisitor.Visit("rank", vehicle.Rank));
            stringBuilder.Append(_propertyVisitor.Visit("skill", vehicle.Skill));
            stringBuilder.Append(_propertyVisitor.Visit("health", vehicle.Health));
            stringBuilder.Append(_propertyVisitor.Visit("fuel", vehicle.Fuel));
            stringBuilder.Append(_propertyVisitor.Visit("ammo", vehicle.Ammo));
            stringBuilder.Append(_propertyVisitor.Visit("text", vehicle.Text));
            stringBuilder.Append(_propertyVisitor.Visit("markers", vehicle.Markers));
            stringBuilder.Append(_propertyVisitor.Visit("init", vehicle.Init));
            stringBuilder.Append(_propertyVisitor.Visit("description", vehicle.Description));
            stringBuilder.Append(_propertyVisitor.Visit("synchronizations", vehicle.Synchronizations));

            stringBuilder.Append(Visit("Vehicles", vehicle.Vehicles));
            stringBuilder.Append(Visit("Waypoints", vehicle.Waypoints));

            stringBuilder.Append("};\n");

            return stringBuilder.ToString();
        }

        public string Visit(string elementName, Waypoint waypoint)
        {
            if (waypoint == null)
            {
                return "";
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("class " + elementName + waypoint.Number + "\n");
            stringBuilder.Append("{\n");
            stringBuilder.Append(_propertyVisitor.Visit("position", waypoint.Position));
            stringBuilder.Append(_propertyVisitor.Visit("placement", waypoint.Placement));
            stringBuilder.Append(_propertyVisitor.Visit("completitionRadius", waypoint.CompletitionRadius));
            stringBuilder.Append(_propertyVisitor.Visit("id", waypoint.Id));
            stringBuilder.Append(_propertyVisitor.Visit("idStatic", waypoint.IdStatic));
            stringBuilder.Append(_propertyVisitor.Visit("idObject", waypoint.IdObject));
            stringBuilder.Append(_propertyVisitor.Visit("housePos", waypoint.HousePos));
            stringBuilder.Append(_propertyVisitor.Visit("type", waypoint.Type));
            stringBuilder.Append(_propertyVisitor.Visit("combatMode", waypoint.CombatMode));
            stringBuilder.Append(_propertyVisitor.Visit("formation", waypoint.Formation));
            stringBuilder.Append(_propertyVisitor.Visit("speed", waypoint.Speed));
            stringBuilder.Append(_propertyVisitor.Visit("combat", waypoint.Combat));
            stringBuilder.Append(_propertyVisitor.Visit("description", waypoint.Description));
            stringBuilder.Append(_propertyVisitor.Visit("expCond", waypoint.ExpCond));
            stringBuilder.Append(_propertyVisitor.Visit("expActiv", waypoint.ExpActiv));
            stringBuilder.Append(_propertyVisitor.Visit("visible", waypoint.Visible));
            stringBuilder.Append(_propertyVisitor.Visit("synchronizations", waypoint.Synchronizations));
            stringBuilder.Append(_propertyVisitor.VisitEffects(waypoint.Effects));
            stringBuilder.Append(_propertyVisitor.Visit("timeoutMin", waypoint.TimeoutMin));
            stringBuilder.Append(_propertyVisitor.Visit("timeoutMid", waypoint.TimeoutMid));
            stringBuilder.Append(_propertyVisitor.Visit("timeoutMax", waypoint.TimeoutMax));
            stringBuilder.Append(_propertyVisitor.Visit("showWP", waypoint.ShowWp));
            stringBuilder.Append("};\n");

            return stringBuilder.ToString();
        }

        public string Visit(string elementName, Marker marker)
        {
            if (marker == null)
            {
                return "";
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("class " + elementName + marker.Number + "\n");
            stringBuilder.Append("{\n");
            stringBuilder.Append(_propertyVisitor.Visit("position", marker.Position));
            stringBuilder.Append(_propertyVisitor.Visit("name", marker.Name));
            stringBuilder.Append(_propertyVisitor.Visit("text", marker.Text));
            stringBuilder.Append(_propertyVisitor.Visit("markerType", marker.MarkerType));
            stringBuilder.Append(_propertyVisitor.Visit("type", marker.Type));
            stringBuilder.Append(_propertyVisitor.Visit("colorName", marker.ColorName));
            stringBuilder.Append(_propertyVisitor.Visit("fillName", marker.FillName));
            stringBuilder.Append(_propertyVisitor.Visit("a", marker.A));
            stringBuilder.Append(_propertyVisitor.Visit("b", marker.B));
            stringBuilder.Append(_propertyVisitor.Visit("angle", marker.Angle));
            stringBuilder.Append(_propertyVisitor.Visit("drawBorder", marker.DrawBorder));
            stringBuilder.Append("};\n");

            return stringBuilder.ToString();
        }

        public string Visit(string elementName, Sensor sensor)
        {
            if (sensor == null)
            {
                return "";
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("class " + elementName + sensor.Number + "\n");
            stringBuilder.Append("{\n");
            stringBuilder.Append(_propertyVisitor.Visit("position", sensor.Position));
            stringBuilder.Append(_propertyVisitor.Visit("a", sensor.A));
            stringBuilder.Append(_propertyVisitor.Visit("b", sensor.B));
            stringBuilder.Append(_propertyVisitor.Visit("angle", sensor.Angle));
            stringBuilder.Append(_propertyVisitor.Visit("rectangular", sensor.Rectangular));
            stringBuilder.Append(_propertyVisitor.Visit("activationBy", sensor.ActivationBy));
            stringBuilder.Append(_propertyVisitor.Visit("activationType", sensor.ActivationType));
            stringBuilder.Append(_propertyVisitor.Visit("repeating", sensor.Repeating));
            stringBuilder.Append(_propertyVisitor.Visit("timeoutMin", sensor.TimeoutMin));
            stringBuilder.Append(_propertyVisitor.Visit("timeoutMid", sensor.TimeoutMid));
            stringBuilder.Append(_propertyVisitor.Visit("timeoutMax", sensor.TimeoutMax));
            stringBuilder.Append(_propertyVisitor.Visit("interruptable", sensor.Interruptable));
            stringBuilder.Append(_propertyVisitor.Visit("type", sensor.Type));
            stringBuilder.Append(_propertyVisitor.Visit("age", sensor.Age));
            stringBuilder.Append(_propertyVisitor.Visit("idVehicle", sensor.IdVehicle));
            stringBuilder.Append(_propertyVisitor.Visit("text", sensor.Text));
            stringBuilder.Append(_propertyVisitor.Visit("name", sensor.Name));
            stringBuilder.Append(_propertyVisitor.Visit("idStatic", sensor.IdStatic));
            stringBuilder.Append(_propertyVisitor.Visit("idObject", sensor.IdObject));
            stringBuilder.Append(_propertyVisitor.Visit("expCond", sensor.ExpCond));
            stringBuilder.Append(_propertyVisitor.Visit("expActiv", sensor.ExpActiv));
            stringBuilder.Append(_propertyVisitor.Visit("expDesactiv", sensor.ExpDesactiv));
            stringBuilder.Append(_propertyVisitor.VisitEffects(sensor.Effects));
            stringBuilder.Append(_propertyVisitor.Visit("synchronizations", sensor.Synchronizations));
            stringBuilder.Append("};\n");

            return stringBuilder.ToString();
        }
    }
}