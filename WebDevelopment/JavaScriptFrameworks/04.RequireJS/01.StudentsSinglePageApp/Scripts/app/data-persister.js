﻿/// <reference path="../libs/jquery-2.0.3.js" />
/// <reference path="../libs/require.js" />
/// <reference path="../libs/rsvp.min.js" />

define(["httpRequester"], function (httpRequester) {
	function getStudents() {		
	    return httpRequester.getJSON("http://localhost:64324/api/students");
	}

	function getMarks(id) {
	    return httpRequester.getJSON("http://localhost:64324/api/students/" + id + "/marks");
	}

	return {
	    getStudents: getStudents,
        getMarks: getMarks
	}
});