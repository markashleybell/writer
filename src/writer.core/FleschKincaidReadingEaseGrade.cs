using System;
using System.Collections.Generic;
using System.Text;

namespace writer.core
{
    public enum FleschKincaidReadingEaseGrade
    {
        /// <summary>
        /// Very easy to read. Easily understood by an average 11-year-old student.
        /// </summary>
        Grade5,

        /// <summary>
        /// Easy to read. Conversational English for consumers.
        /// </summary>
        Grade6,

        /// <summary>
        /// Fairly easy to read.
        /// </summary>
        Grade7,

        /// <summary>
        /// 	Plain English. Easily understood by 13- to 15-year-old students.
        /// </summary>
        Grade8To9,

        /// <summary>
        /// Fairly difficult to read.
        /// </summary>
        Grade10To12,

        /// <summary>
        /// Difficult to read.
        /// </summary>
        College,

        /// <summary>
        /// Very difficult to read. Best understood by university graduates.
        /// </summary>
        CollegeGraduate,

        /// <summary>
        /// Extremely difficult to read. Best understood by university graduates.
        /// </summary>
        Professional
    }
}
