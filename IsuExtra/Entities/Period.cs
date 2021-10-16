using System;

namespace IsuExtra.Entities
{
    public class Period
    {
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        private Professor _professor;
        private uint _classroom;

        public Period(DateTime startTime, Professor professor, uint classroom,  double duration = 1.5)
        {
            _startTime = startTime;
            _endTime = _startTime + TimeSpan.FromHours(duration);
            _professor = professor;
            _classroom = classroom;
        }

        public bool DoesIntersect(DateTime startTime, DateTime endTime)
        {
            return startTime < _endTime && endTime > _startTime;
        }

        public bool DoesIntersect(Period period) => period.DoesIntersect(_startTime, _endTime);
    }
}