//-----------------------------------------------------------------------
// <copyright file="ProjectVersion.cs" company="Victor Boctor">
//     Copyright (C) All Rights Reserved
// </copyright>
// <summary>
// MantisConnect is copyrighted to Victor Boctor
//
// This program is distributed under the terms and conditions of the GPL
// See LICENSE file for details.
//
// For commercial applications to link with or modify MantisConnect, they require the
// purchase of a MantisConnect commercial license.
// </summary>
//-----------------------------------------------------------------------

namespace Futureware.MantisConnect
{
    using System;

    /// <summary>
	/// A class that includes information relating to project versions.
	/// </summary>
    [Serializable]
    public sealed class ProjectVersion
	{
        /// <summary>
        /// The proejct version id.
        /// </summary>
        private int id;

        /// <summary>
        /// The project version name.
        /// </summary>
        private string name;

        /// <summary>
        /// The project id.
        /// </summary>
        private int projectId;

        /// <summary>
        /// The date the version was created / released.
        /// </summary>
        private DateTime dateOrder;

        /// <summary>
        /// The project version description.
        /// </summary>
        private string description;

        /// <summary>
        /// A flag indicating whether the project version is released or not.
        /// </summary>
        private bool isReleased;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectVersion"/> class.
        /// </summary>
		public ProjectVersion()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectVersion"/> class.
        /// </summary>
        /// <param name="projectVersionData">Project version data.</param>
		internal ProjectVersion(MantisConnectWebservice.ProjectVersionData projectVersionData)
		{
			this.Id = Convert.ToInt32(projectVersionData.id);
			this.Name = projectVersionData.name;
			this.ProjectId = Convert.ToInt32(projectVersionData.project_id);
			this.DateOrder = projectVersionData.date_order;
			this.Description = projectVersionData.description;
			this.IsReleased = projectVersionData.released;
		}

        /// <summary>
        /// Convert this instance to the type supported by the webservice proxy.
        /// </summary>
        /// <returns>A copy of this instance in the webservice proxy type.</returns>
        internal MantisConnectWebservice.ProjectVersionData ToWebservice()
        {
            MantisConnectWebservice.ProjectVersionData projectVersionData = new MantisConnectWebservice.ProjectVersionData();

            projectVersionData.id = this.Id.ToString();
            projectVersionData.project_id = this.ProjectId.ToString();
            projectVersionData.description = this.Description;
            projectVersionData.name = this.Name;
            projectVersionData.released = this.IsReleased;
            projectVersionData.releasedSpecified = true;
            projectVersionData.date_order = this.DateOrder;
            projectVersionData.date_orderSpecified = true;

            return projectVersionData;
        }

        /// <summary>
        /// Converts an array of project versions from webservice proxy type to this type.
        /// </summary>
        /// <param name="projectVersionDataArray">Project version data array.</param>
        /// <returns>An array of project versions in this type.</returns>
		internal static ProjectVersion[] ConvertArray(MantisConnectWebservice.ProjectVersionData[] projectVersionDataArray)
		{
            if (projectVersionDataArray == null)
            {
                return null;
            }

			ProjectVersion[] projectVersions = new ProjectVersion[projectVersionDataArray.Length];

            for (int i = 0; i < projectVersionDataArray.Length; ++i)
            {
                projectVersions[i] = new ProjectVersion(projectVersionDataArray[i]);
            }

			return projectVersions;
		}

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>Greater than or equal to 1.</value>
		public int ProjectId
		{
			get { return this.projectId; }
			set { this.projectId = value; }
		}

        /// <summary>
        /// Gets or sets the date order.
        /// </summary>
		public DateTime DateOrder
		{
			get { return this.dateOrder; }
			set { this.dateOrder = value; }
		}

        /// <summary>
        /// Gets or sets the project version description.  This version appears in the changelog.
        /// </summary>
		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

        /// <summary>
        /// Gets or sets a value indicating whether this project version is released.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if this instance is released; otherwise, <see langword="false"/>.
        /// </value>
		public bool IsReleased
		{
			get { return this.isReleased; }
			set { this.isReleased = value; }
		}
	}
}
